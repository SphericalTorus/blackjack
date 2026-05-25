using System.Collections.Generic;

namespace Blackjack.Domain.Cards
{
    public sealed class Deck
    {
        private readonly List<Card> _cards;
        
        public bool IsEmpty => _cards.Count == 0;
        
        public Deck(List<Card> cards)
        {
            _cards = cards;
        }

        public bool TryTakeCard(out Card card)
        {
            if (_cards.Count == 0)
            {
                card = default;
                return false;
            }

            var lastIndex = _cards.Count - 1;
            card = _cards[lastIndex];
            _cards.RemoveAt(lastIndex);
            return true;
        }
    }
}
