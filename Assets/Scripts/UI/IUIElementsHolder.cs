namespace Blackjack.UI
{
    public interface IUIElementsHolder
    {
        MenuScreen MenuScreen { get; }
        GameplayScreen GameplayScreen { get; }
        ResultsScreen ResultsScreen { get; }
    }
}