using System.Collections.Generic;

namespace Blackjack.Domain.Round
{
    public readonly struct RoundResult
    {
        public bool IsDraw { get; }
        public PlayerResult? Winner { get; }
        public IReadOnlyList<PlayerResult> PlayerResults { get; }

        public RoundResult(bool isDraw, PlayerResult? winner, List<PlayerResult> playerResults)
        {
            IsDraw = isDraw;
            Winner = winner;
            PlayerResults = playerResults;
        }
    }
}
