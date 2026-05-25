using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;
using NUnit.Framework;

namespace Blackjack.Tests.Domain
{
    public sealed class DefaultGameRulesTests
    {
        private readonly static PlayerId HumanPlayerId = new("Human");
        private readonly static PlayerId AiPlayerId = new("AI");

        private DefaultGameRules _rules;

        [SetUp]
        public void SetUp()
        {
            _rules = new DefaultGameRules(new TestGameRulesConfig());
        }

        [Test]
        public void CalculateScore_AceCanImproveHand_UsesAceAsEleven()
        {
            var hand = HandWith(Rank.Ace, Rank.Nine);

            var score = _rules.CalculateScore(hand);

            Assert.AreEqual(20, score.Value);
            Assert.IsTrue(score.HasAceValueAsEleven);
            Assert.IsFalse(score.IsBust);
        }

        [Test]
        public void CalculateScore_HighAceWouldBust_UsesAceAsOne()
        {
            var hand = HandWith(Rank.Ace, Rank.Nine, Rank.King);

            var score = _rules.CalculateScore(hand);

            Assert.AreEqual(20, score.Value);
            Assert.IsFalse(score.HasAceValueAsEleven);
            Assert.IsFalse(score.IsBust);
        }

        [Test]
        public void TryCalculateRoundResult_StandingPlayersHaveSameScore_ReturnsDraw()
        {
            var human = PlayerWith(HumanPlayerId, Rank.Ten, Rank.Seven);
            var ai = PlayerWith(AiPlayerId, Rank.Nine, Rank.Eight);
            human.Stand();
            ai.Stand();

            var finished = _rules.TryCalculateRoundResult(
                new List<PlayerState> { human, ai },
                isDeckEmpty: false,
                out var roundResult);

            Assert.IsTrue(finished);
            Assert.IsTrue(roundResult.IsDraw);
            Assert.IsFalse(roundResult.Winner.HasValue);
        }

        [Test]
        public void TryCalculateRoundResult_OnlyOnePlayerAvoidsBust_EndsRoundWithWinner()
        {
            var human = PlayerWith(HumanPlayerId, Rank.King, Rank.Queen, Rank.Two);
            var ai = PlayerWith(AiPlayerId, Rank.Nine, Rank.Eight);

            var finished = _rules.TryCalculateRoundResult(
                new List<PlayerState> { human, ai },
                isDeckEmpty: false,
                out var roundResult);

            Assert.IsTrue(finished);
            Assert.IsFalse(roundResult.IsDraw);
            Assert.IsTrue(roundResult.Winner.HasValue);
            Assert.AreEqual(AiPlayerId, roundResult.Winner.Value.PlayerId);
        }

        private static Hand HandWith(params Rank[] ranks)
        {
            var hand = new Hand();

            foreach (var rank in ranks)
            {
                hand.Add(new Card(rank, Suit.Clubs));
            }

            return hand;
        }

        private static PlayerState PlayerWith(PlayerId playerId, params Rank[] ranks)
        {
            var player = new PlayerState(playerId);

            foreach (var rank in ranks)
            {
                player.Hand.Add(new Card(rank, Suit.Clubs));
            }

            return player;
        }
    }
}
