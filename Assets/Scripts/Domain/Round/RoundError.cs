namespace Blackjack.Domain.Round
{
    public readonly struct RoundError
    {
        public RoundErrorType Type { get; }
        public string Message { get; }

        public RoundError(RoundErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        public override string ToString()
        {
            return $"{Type} {Message}";
        }
    }
}
