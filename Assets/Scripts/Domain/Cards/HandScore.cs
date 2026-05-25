namespace Blackjack.Domain.Cards
{
    public readonly struct HandScore
    {
        public int Value { get; }
        public bool HasAceValueAsEleven { get; }
        public bool IsBust { get; }

        public HandScore(int value, bool hasAceValuedAsEleven, bool isBust)
        {
            Value = value;
            HasAceValueAsEleven = hasAceValuedAsEleven;
            IsBust = isBust;
        }
    }
}