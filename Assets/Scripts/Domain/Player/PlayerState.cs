using Blackjack.Domain.Cards;

namespace Blackjack.Domain.Player
{
    public sealed class PlayerState
    {
        public PlayerId PlayerId { get; }
        public Hand Hand { get; } = new();
        public bool IsStanding { get; private set; }

        public PlayerState(PlayerId playerId)
        {
            PlayerId = playerId;
        }

        public void Stand()
        {
            IsStanding = true;
        }
    }
}
