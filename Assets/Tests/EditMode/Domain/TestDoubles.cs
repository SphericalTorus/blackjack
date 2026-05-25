using System;
using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Tools;

namespace Blackjack.Tests.Domain
{
    internal sealed class TestGameRulesConfig : IGameRulesConfig
    {
        public string Name { get; set; } = "Test Blackjack";
        public int MinPlayerCount { get; set; } = 2;
        public int MaxPlayerCount { get; set; } = 2;
        public int TargetScore { get; set; } = 21;
        public int InitialCardsPerPlayer { get; set; } = 2;
        public int FaceCardValue { get; set; } = 10;
        public int AceLowValue { get; set; } = 1;
        public int AceHighValue { get; set; } = 11;
    }

    internal sealed class OrderedCardShuffler : ICardShuffler
    {
        private readonly IReadOnlyList<Card> _drawOrder;

        public OrderedCardShuffler(IReadOnlyList<Card> drawOrder)
        {
            _drawOrder = drawOrder;
        }

        public void Shuffle(IList<Card> cards)
        {
            cards.Clear();

            for (var i = _drawOrder.Count - 1; i >= 0; i--)
            {
                cards.Add(_drawOrder[i]);
            }
        }
    }

    internal sealed class FixedRandom : IRandom
    {
        private readonly Queue<int> _values;

        public FixedRandom(params int[] values)
        {
            _values = new Queue<int>(values);
        }

        public double NextDouble()
        {
            return 0d;
        }

        public int Next(int maxValue)
        {
            if (_values.Count == 0)
            {
                return 0;
            }

            var value = _values.Dequeue();

            if (value < 0 || value >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    $"Fixed random value must be between 0 and {maxValue - 1}.");
            }

            return value;
        }
    }
}
