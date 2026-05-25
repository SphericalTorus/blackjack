using Blackjack.Domain.Cards;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.AI
{
    public sealed class SimpleAiStrategy : IAiStrategy
    {
        private readonly IGameRules _gameRules;

        public SimpleAiStrategy(IGameRules gameRules)
        {
            _gameRules = gameRules;
        }

        public PlayerAction DecideAction(Hand aiHand, Hand opponentHand)
        {
            return ShouldTakeCard(aiHand, opponentHand)
                ? PlayerAction.TakeCard
                : PlayerAction.Stand;
        }

        private bool ShouldTakeCard(Hand aiHand, Hand opponentHand)
        {
            var aiScore = _gameRules.CalculateScore(aiHand);

            return aiScore.HasAceValueAsEleven
                ? ShouldTakeCardWithSoftAce(aiHand, opponentHand, aiScore)
                : ShouldTakeCardWithoutSoftAce(opponentHand, aiScore);
        }

        // Simplified blackjack basic strategy: opponent hand is treated as the visible threat.
        private bool ShouldTakeCardWithoutSoftAce(Hand opponentHand, HandScore aiScore)
        {
            if (HasAceOrCardWithAtLeastValue(opponentHand, minValue: 7))
            {
                return aiScore.Value < 17;
            }

            if (HasCardWithValueInRange(opponentHand, minValue: 4, maxValue: 6))
            {
                return aiScore.Value < 12;
            }

            if (HasCardWithValueInRange(opponentHand, minValue: 2, maxValue: 3))
            {
                return aiScore.Value < 13;
            }

            return false;
        }

        private bool ShouldTakeCardWithSoftAce(Hand aiHand, Hand opponentHand, HandScore aiScore)
        {
            if (aiScore.Value >= 19)
            {
                return false;
            }

            if (aiScore.Value == 18 &&
                aiHand.Cards.Count >= 3 &&
                !HasAceOrCardWithAtLeastValue(opponentHand, minValue: 9))
            {
                return false;
            }

            return true;
        }

        private bool HasAceOrCardWithAtLeastValue(Hand hand, int minValue)
        {
            var cards = hand.Cards;

            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];

                if (card.Rank == Rank.Ace || _gameRules.GetSingleCardValue(card.Rank) >= minValue)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasCardWithValueInRange(Hand hand, int minValue, int maxValue)
        {
            var cards = hand.Cards;

            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                var cardValue = _gameRules.GetSingleCardValue(card.Rank);

                if (cardValue >= minValue && cardValue <= maxValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
