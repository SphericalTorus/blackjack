namespace Blackjack.UI
{
    public interface IResultsScreenViewModel
    {
        string ResultText { get; }

        void HandleToMenuButtonClick();
        void HandlePlayAgainButtonClick();
    }
}