namespace Blackjack.UI
{
    public sealed class UIModel
    {
        private readonly IUIElementsHolder _uiElementsHolder;

        public UIModel(IUIElementsHolder uiElementsHolder)
        {
            _uiElementsHolder = uiElementsHolder;

            HideMenuScreen();
            HideGameplayScreen();
        }
        
        public void ShowMenuScreen(IMenuScreenViewModel viewModel)
        {
            _uiElementsHolder.MenuScreen.gameObject.SetActive(true);
            _uiElementsHolder.MenuScreen.Init(viewModel);
        }

        public void HideMenuScreen()
        {
            _uiElementsHolder.MenuScreen.gameObject.SetActive(false);
        }

        public void ShowGameplayScreen(IGameplayScreenViewModel viewModel)
        {
            _uiElementsHolder.GameplayScreen.gameObject.SetActive(true);
            _uiElementsHolder.GameplayScreen.Init(viewModel);
        }

        public void HideGameplayScreen()
        {
            _uiElementsHolder.GameplayScreen.OnHide();
            _uiElementsHolder.GameplayScreen.gameObject.SetActive(false);
        }

        public void ShowResultsScreen(IResultsScreenViewModel viewModel)
        {
            _uiElementsHolder.ResultsScreen.gameObject.SetActive(true);
            _uiElementsHolder.ResultsScreen.Init(viewModel);
        }

        public void HideResultsScreen()
        {
            _uiElementsHolder.ResultsScreen.gameObject.SetActive(false);
        }
    }
}