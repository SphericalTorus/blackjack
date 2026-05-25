using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round.Rules
{
    public interface IGameRules
    {
        int MinPlayersCount { get; }
        int MaxPlayersCount { get; }
        int InitialCardsPerPlayer { get; }

        int GetSingleCardValue(Rank rank);
        HandScore CalculateScore(Hand hand);
        bool IsBusted(Hand hand);
        bool TryCalculateRoundResult(IReadOnlyList<PlayerState> players, bool isDeckEmpty, out RoundResult roundResult);
    }
}
