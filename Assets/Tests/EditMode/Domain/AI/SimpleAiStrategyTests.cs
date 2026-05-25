using Blackjack.Domain.AI;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;
using NUnit.Framework;

namespace Blackjack.Tests.Domain.AI
{
    public sealed class SimpleAiStrategyTests
    {
        private SimpleAiStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new SimpleAiStrategy(new DefaultGameRules(new TestGameRulesConfig()));
        }

        [Test]
        public void DecideAction_NoSoftAceAndOpponentHasSeven_TakesCardBelowSeventeen()
        {
            var action = _strategy.DecideAction(
                HandWith(Rank.Ten, Rank.Six),
                HandWith(Rank.Seven, Rank.Two));

            Assert.AreEqual(PlayerAction.TakeCard, action);
        }

        [Test]
        public void DecideAction_NoSoftAceAndOpponentHasSeven_StandsAtSeventeen()
        {
            var action = _strategy.DecideAction(
                HandWith(Rank.Ten, Rank.Seven),
                HandWith(Rank.Seven, Rank.Two));

            Assert.AreEqual(PlayerAction.Stand, action);
        }

        [Test]
        public void DecideAction_SoftAce_StandsAtNineteen()
        {
            var action = _strategy.DecideAction(
                HandWith(Rank.Ace, Rank.Eight),
                HandWith(Rank.King, Rank.Two));

            Assert.AreEqual(PlayerAction.Stand, action);
        }

        [Test]
        public void DecideAction_SoftEighteenWithThreeCards_TakesCardAgainstStrongOpponent()
        {
            var action = _strategy.DecideAction(
                HandWith(Rank.Ace, Rank.Four, Rank.Three),
                HandWith(Rank.Nine, Rank.Two));

            Assert.AreEqual(PlayerAction.TakeCard, action);
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
    }
}
