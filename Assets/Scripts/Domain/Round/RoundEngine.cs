using System;
using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round.Events;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Round.Validation;
using Blackjack.Domain.Player;
using Blackjack.Domain.Tools;

namespace Blackjack.Domain.Round
{
    public sealed class RoundEngine
    {
        private readonly IGameRules _gameRules;
        private readonly IRandom _random;
        private readonly DeckFactory _deckFactory;
        private readonly RoundCommandValidator _commandValidator;
        private RoundState _roundState;

        public RoundEngine(IGameRules gameRules, DeckFactory deckFactory, IRandom random)
        {
            _gameRules = gameRules;
            _random = random;
            _deckFactory = deckFactory;
            _commandValidator = new RoundCommandValidator();
        }
        
        public RoundCommandResult StartRound(IReadOnlyList<PlayerId> playerIds)
        {
            var validationResult = _commandValidator.ValidateStartRound(playerIds, _roundState, _gameRules);
            
            if (validationResult.Status != RoundCommandStatus.Success)
            {
                return validationResult;
            }
            
            var deck = _deckFactory.Create();
            var players = new List<PlayerState>(playerIds.Count);

            for (var playerIdIndex = 0; playerIdIndex < playerIds.Count; playerIdIndex++)
            {
                var playerId = playerIds[playerIdIndex];
                var playerState = new PlayerState(playerId);
                
                // At this point deck always has enough cards: guaranteed by game rules config validation.
                for (var i = 0; i < _gameRules.InitialCardsPerPlayer; i++)
                {
                    deck.TryTakeCard(out var card);
                    playerState.Hand.Add(card);
                }
                
                players.Add(playerState);
            }
            
            var firstTurnPlayer = players[_random.Next(players.Count)];
            _roundState = new RoundState(players, deck, firstTurnPlayer.PlayerId);
            TryFinishRound();

            return new RoundCommandResult(
                RoundCommandStatus.Success,
                _roundState,
                new RoundEvents(_roundState.CurrentPlayerId, _roundState.RoundStatus));
        }

        public void ResetRound()
        {
            _roundState = null;
        }

        public RoundCommandResult ExecutePlayerCommand(PlayerCommand command)
        {
            var validationResult = _commandValidator.ValidatePlayerCommand(command, _roundState, out var playerState);
            
            if (validationResult.Status != RoundCommandStatus.Success)
            {
                return validationResult;
            }

            switch (command.PlayerAction)
            {
                case PlayerAction.TakeCard:
                    if (_roundState.Deck.TryTakeCard(out var card))
                    {
                        playerState.Hand.Add(card);
                    }
                    break;
                case PlayerAction.Stand:
                    playerState.Stand();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var events = RoundEvents.Empty;

            if (TryFinishRound())
            {
                events = new RoundEvents(_roundState.RoundStatus);
            }
            else if (_roundState.RoundStatus == RoundStatus.Playing)
            {
                _roundState.MoveToNextPlayer(CanPlayerTakeTurn);
                events = new RoundEvents(_roundState.CurrentPlayerId);
            }

            return new RoundCommandResult(RoundCommandStatus.Success, _roundState, events);
        }

        private bool CanPlayerTakeTurn(PlayerState player)
        {
            return !player.IsStanding && !_gameRules.IsBusted(player.Hand);
        }

        private bool TryFinishRound()
        {
            if (_roundState.RoundResult.HasValue)
            {
                return false;
            }

            if (_gameRules.TryCalculateRoundResult(_roundState.Players, _roundState.Deck.IsEmpty, out var roundResult))
            {
                _roundState.SetRoundResult(roundResult);
                _roundState.SetRoundStatus(RoundStatus.Finished);

                return true;
            }

            return false;
        }
    }
}
