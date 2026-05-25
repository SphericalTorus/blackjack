namespace Blackjack.Domain.Cards
{
    public readonly struct Card
    {
        public const int UniqueCardsCount =
            ((int)Rank.King - (int)Rank.Ace + 1) *
            ((int)Suit.Spades - (int)Suit.Clubs + 1);

        public Rank Rank { get; }
        public Suit Suit { get; }

        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
