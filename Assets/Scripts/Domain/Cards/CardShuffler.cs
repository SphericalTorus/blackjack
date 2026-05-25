using System.Collections.Generic;
using Blackjack.Domain.Tools;

namespace Blackjack.Domain.Cards
{
    public sealed class CardShuffler : ICardShuffler
    {
        private readonly IRandom _random;

        public CardShuffler(IRandom random)
        {
            _random = random;
        }
        
        public void Shuffle(IList<Card> cards)
        {
            var n = cards.Count;
        
            while (n > 1)
            {
                n--;
                var k = _random.Next(n + 1);
                (cards[k], cards[n]) = (cards[n], cards[k]);
            }
        }
    }
}