using Blackjack.Domain.Player;

namespace Blackjack.Gameplay.DTO
{
    public sealed record PlayerResultDto(PlayerId PlayerId, int ScoreValue, bool IsBust);
}
