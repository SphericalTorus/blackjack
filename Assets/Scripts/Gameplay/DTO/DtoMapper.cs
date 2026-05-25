using System.Collections.Generic;
using Blackjack.Domain.Round;

namespace Blackjack.Gameplay.DTO
{
    public sealed class DtoMapper
    {
        public RoundResultDto Map(RoundResult roundResult)
        {
            var winnerDto = roundResult.Winner.HasValue 
                ? new PlayerResultDto(
                    roundResult.Winner.Value.PlayerId,
                    roundResult.Winner.Value.Score.Value,
                    roundResult.Winner.Value.Score.IsBust)
                : null;
            
            var playerResultsDto = new List<PlayerResultDto>(roundResult.PlayerResults.Count);

            foreach (var playerResult in roundResult.PlayerResults)
            {
                playerResultsDto.Add(new PlayerResultDto(
                    playerResult.PlayerId,
                    playerResult.Score.Value,
                    playerResult.Score.IsBust));
            }

            return new RoundResultDto(roundResult.IsDraw, winnerDto, playerResultsDto);
        }
    }
}
