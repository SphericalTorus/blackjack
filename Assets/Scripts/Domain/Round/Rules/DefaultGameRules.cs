using System;
using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round.Rules
{
    public sealed class DefaultGameRules : IGameRules
    {
        private readonly IGameRulesConfig _config;

        public DefaultGameRules(IGameRulesConfig config)
        {
            _config = config;
        }

        public string Name => _config.Name;
        public int MinPlayersCount => _config.MinPlayerCount;
        public int MaxPlayersCount => _config.MaxPlayerCount;
        public int InitialCardsPerPlayer => _config.InitialCardsPerPlayer;

        public int GetSingleCardValue(Rank rank)
        {
            return rank switch
            {
                Rank.Ace => _config.AceHighValue,
                Rank.Two => 2,
                Rank.Three => 3,
                Rank.Four => 4,
                Rank.Five => 5,
                Rank.Six => 6,
                Rank.Seven => 7,
                Rank.Eight => 8,
                Rank.Nine => 9,
                Rank.Ten => 10,
                Rank.Jack or Rank.Queen or Rank.King => _config.FaceCardValue,
                _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
            };
        }

        // The round ends early when only one or no players can still win;
        // otherwise it waits until all active players stand or the deck is exhausted.
        public bool TryCalculateRoundResult(IReadOnlyList<PlayerState> players, bool isDeckEmpty, out RoundResult roundResult)
        {
            PlayerResult? bestPlayerResult = null;
            var notBustedPlayersCount = 0;
            var bestScorePlayersCount = 0;
            var areAllPlayersStanding = true;

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var score = CalculateScore(player.Hand);
                var playerResult = new PlayerResult(player.PlayerId, score);
                
                areAllPlayersStanding &= player.IsStanding;

                if (score.IsBust)
                {
                    continue;
                }

                notBustedPlayersCount++;

                if (!bestPlayerResult.HasValue ||
                    bestPlayerResult.Value.Score.Value < playerResult.Score.Value)
                {
                    bestPlayerResult = playerResult;
                    bestScorePlayersCount = 1;
                }
                else if (bestPlayerResult.Value.Score.Value == playerResult.Score.Value)
                {
                    bestScorePlayersCount++;
                }
            }

            if (notBustedPlayersCount == 0)
            {
                roundResult = new RoundResult(isDraw: true, winner: null, BuildPlayerResults(players));
                return true;
            }

            if (notBustedPlayersCount == 1)
            {
                roundResult = new RoundResult(isDraw: false, winner: bestPlayerResult, BuildPlayerResults(players));
                return true;
            }

            if (!areAllPlayersStanding && !isDeckEmpty)
            {
                roundResult = default;
                return false;
            }

            if (bestScorePlayersCount > 1)
            {
                roundResult = new RoundResult(isDraw: true, winner: null, BuildPlayerResults(players));
                return true;
            }

            roundResult = new RoundResult(isDraw: false, winner: bestPlayerResult, BuildPlayerResults(players));
            return true;
        }

        public bool IsBusted(Hand hand)
        {
            return CalculateScore(hand).IsBust;
        }

        // Score all aces as low first, then promote as many as possible without busting.
        public HandScore CalculateScore(Hand hand)
        {
            var total = CalculateMinimumScore(hand, out var aceCount);

            var hasAceAsEleven = false;
            var aceScoreDelta = _config.AceHighValue - _config.AceLowValue;

            while (aceCount > 0 && total + aceScoreDelta <= _config.TargetScore)
            {
                total += aceScoreDelta;
                aceCount--;
                hasAceAsEleven = true;
            }

            return new HandScore(
                total,
                hasAceAsEleven,
                total > _config.TargetScore
            );
        }

        private int CalculateMinimumScore(Hand hand, out int aceCount)
        {
            var total = 0;
            aceCount = 0;

            var cards = hand.Cards;

            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];

                if (card.Rank == Rank.Ace)
                {
                    total += _config.AceLowValue;
                    aceCount++;
                }
                else
                {
                    total += GetSingleCardValue(card.Rank);
                }
            }

            return total;
        }

        private List<PlayerResult> BuildPlayerResults(IReadOnlyList<PlayerState> players)
        {
            var playerResults = new List<PlayerResult>(players.Count);

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                playerResults.Add(new PlayerResult(player.PlayerId, CalculateScore(player.Hand)));
            }

            return playerResults;
        }
    }
}
