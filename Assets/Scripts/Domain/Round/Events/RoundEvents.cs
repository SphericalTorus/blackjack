using Blackjack.Domain.Round;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round.Events
{
    public readonly struct RoundEvents
    {
        public static RoundEvents Empty => default;

        public bool HasCurrentPlayerChanged { get; }
        public PlayerId CurrentPlayerId { get; }
        public bool HasRoundStatusChanged { get; }
        public RoundStatus RoundStatus { get; }

        public RoundEvents(PlayerId currentPlayerId)
        {
            HasCurrentPlayerChanged = true;
            CurrentPlayerId = currentPlayerId;
            HasRoundStatusChanged = false;
            RoundStatus = default;
        }

        public RoundEvents(RoundStatus roundStatus)
        {
            HasCurrentPlayerChanged = false;
            CurrentPlayerId = default;
            HasRoundStatusChanged = true;
            RoundStatus = roundStatus;
        }

        public RoundEvents(PlayerId currentPlayerId, RoundStatus roundStatus)
        {
            HasCurrentPlayerChanged = true;
            CurrentPlayerId = currentPlayerId;
            HasRoundStatusChanged = true;
            RoundStatus = roundStatus;
        }
    }
}
