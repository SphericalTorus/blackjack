using Blackjack.Domain.Cards;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round
{
    public readonly struct PlayerResult
    {
        public PlayerId PlayerId { get; }
        public HandScore Score { get; }

        public PlayerResult(PlayerId playerId, HandScore score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }
}
