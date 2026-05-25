namespace Blackjack.Domain.Round.Rules
{
    public interface IGameRulesConfig
    {
        string Name { get; }
        int MinPlayerCount { get; } 
        int MaxPlayerCount { get; }
        int TargetScore { get; }
        int InitialCardsPerPlayer { get; }
        int FaceCardValue { get; }
        int AceLowValue { get; }
        int AceHighValue { get; }
    }
}