using Blackjack.Domain.Cards;
using Blackjack.Domain.Round;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;
using NUnit.Framework;

namespace Blackjack.Tests.Domain
{
    public sealed class RoundEngineTests
    {
        private readonly static PlayerId HumanPlayerId = new("Human");
        private readonly static PlayerId AiPlayerId = new("AI");

        [Test]
        public void StartRound_WithInjectedRandom_DealsInitialCardsAndSetsFirstTurn()
        {
            var roundEngine = CreateRoundEngine(
                firstPlayerIndex: 1,
                Card(Rank.Two),
                Card(Rank.Three),
                Card(Rank.Four),
                Card(Rank.Five),
                Card(Rank.Six));

            var result = roundEngine.StartRound(new[] { HumanPlayerId, AiPlayerId });

            Assert.AreEqual(RoundCommandStatus.Success, result.Status);
            Assert.AreEqual(AiPlayerId, result.RoundState.CurrentPlayerId);
            Assert.AreEqual(2, result.RoundState.Players[0].Hand.Cards.Count);
            Assert.AreEqual(2, result.RoundState.Players[1].Hand.Cards.Count);
            Assert.AreEqual(RoundStatus.Playing, result.RoundState.RoundStatus);
        }

        [Test]
        public void ExecutePlayerCommand_PlayerDoesNotHaveTurn_ReturnsWrongTurnFailure()
        {
            var roundEngine = CreateRoundEngine(
                firstPlayerIndex: 0,
                Card(Rank.Two),
                Card(Rank.Three),
                Card(Rank.Four),
                Card(Rank.Five),
                Card(Rank.Six));
            roundEngine.StartRound(new[] { HumanPlayerId, AiPlayerId });

            var result = roundEngine.ExecutePlayerCommand(new PlayerCommand(AiPlayerId, PlayerAction.Stand));

            Assert.AreEqual(RoundCommandStatus.Failure, result.Status);
            Assert.AreEqual(RoundErrorType.WrongTurn, result.Error.Type);
            Assert.AreEqual(RoundStatus.Playing, result.RoundState.RoundStatus);
        }

        [Test]
        public void ExecutePlayerCommand_CurrentPlayerBusts_FinishesRoundImmediately()
        {
            var roundEngine = CreateRoundEngine(
                firstPlayerIndex: 0,
                Card(Rank.King),
                Card(Rank.Queen),
                Card(Rank.Nine),
                Card(Rank.Eight),
                Card(Rank.Two));
            roundEngine.StartRound(new[] { HumanPlayerId, AiPlayerId });

            var result = roundEngine.ExecutePlayerCommand(new PlayerCommand(HumanPlayerId, PlayerAction.TakeCard));

            Assert.AreEqual(RoundCommandStatus.Success, result.Status);
            Assert.AreEqual(RoundStatus.Finished, result.RoundState.RoundStatus);
            Assert.IsTrue(result.RoundState.RoundResult.HasValue);
            Assert.IsTrue(result.RoundState.RoundResult.Value.Winner.HasValue);
            Assert.AreEqual(AiPlayerId, result.RoundState.RoundResult.Value.Winner.Value.PlayerId);
        }

        private static RoundEngine CreateRoundEngine(int firstPlayerIndex, params Card[] drawOrder)
        {
            var rules = new DefaultGameRules(new TestGameRulesConfig());
            var deckFactory = new DeckFactory(new OrderedCardShuffler(drawOrder));

            return new RoundEngine(rules, deckFactory, new FixedRandom(firstPlayerIndex));
        }

        private static Card Card(Rank rank)
        {
            return new Card(rank, Suit.Clubs);
        }
    }
}
