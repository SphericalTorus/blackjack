using Blackjack.Gameplay.PlaySession;

namespace Blackjack.UI
{
    public sealed class MenuScreenViewModel : IMenuScreenViewModel
    {
        private readonly UIModel _uiModel;
        private readonly PlaySessionModel _playSessionModel;
        private readonly ApplicationModel _applicationModel;

        public MenuScreenViewModel(UIModel uiModel, PlaySessionModel playSessionModel, ApplicationModel applicationModel)
        {
            _uiModel = uiModel;
            _playSessionModel = playSessionModel;
            _applicationModel = applicationModel;
        }
        
        public void HandlePlayButtonClick()
        {
            _uiModel.HideMenuScreen();
            _uiModel.ShowGameplayScreen(new GameplayScreenViewModel(_uiModel, _playSessionModel, _applicationModel));

            if (!_playSessionModel.StartSession())
            {
                _uiModel.HideGameplayScreen();
                _uiModel.ShowMenuScreen(new MenuScreenViewModel(_uiModel, _playSessionModel, _applicationModel));
            }
        }

        public void HandleExitButtonClick()
        {
            _applicationModel.ExitApplication();
        }
    }
}
