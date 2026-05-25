using System.Collections.Generic;

namespace Blackjack.Domain.Cards
{
    public sealed class Hand
    {
        private readonly List<Card> _cards = new();
        
        public IReadOnlyList<Card> Cards => _cards;
        
        public void Add(Card card)
        {
            _cards.Add(card);
        }
    }
}