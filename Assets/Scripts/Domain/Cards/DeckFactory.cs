using System.Collections.Generic;

namespace Blackjack.Domain.Cards
{
    public sealed class DeckFactory
    {
        private readonly ICardShuffler _cardShuffler;

        public DeckFactory(ICardShuffler cardShuffler)
        {
            _cardShuffler = cardShuffler;
        }
        
        public Deck Create()
        {
            var cards = new List<Card>(Card.UniqueCardsCount);

            for (var rankValue = (int)Rank.Ace; rankValue <= (int)Rank.King; rankValue++)
            {
                var rank = (Rank)rankValue;

                for (var suitValue = (int)Suit.Clubs; suitValue <= (int)Suit.Spades; suitValue++)
                {
                    cards.Add(new Card(rank, (Suit)suitValue));
                }
            }

            _cardShuffler.Shuffle(cards);

            return new Deck(cards);
        }
    }
}
