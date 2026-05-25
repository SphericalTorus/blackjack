using System.Collections.Generic;

namespace Blackjack.Domain.Cards
{
    public interface ICardShuffler
    {
        void Shuffle(IList<Card> cards);
    }
}