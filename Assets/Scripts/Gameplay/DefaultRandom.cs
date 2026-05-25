using System;
using Blackjack.Domain.Tools;

namespace Blackjack.Gameplay
{
    public sealed class DefaultRandom : IRandom
    {
        private readonly Random _random = new();
        
        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}