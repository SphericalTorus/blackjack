using Blackjack.Domain.Round.Events;

namespace Blackjack.Domain.Round
{
    public readonly struct RoundCommandResult
    {
        public RoundCommandStatus Status { get; }
        public RoundError Error { get; }
        public RoundState RoundState { get; }
        public RoundEvents Events { get; }

        public RoundCommandResult(RoundCommandStatus status, RoundState roundState, RoundEvents events = default, RoundError error = default)
        {
            Status = status;
            Error = error;
            RoundState = roundState;
            Events = events;
        }
    }
}
