using System.Collections.Generic;

namespace Blackjack.Gameplay.DTO
{
    public sealed record RoundResultDto(bool IsDraw, PlayerResultDto Winner, List<PlayerResultDto> PlayerResults);
}