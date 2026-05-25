using Blackjack.Gameplay.DTO;
using Blackjack.Gameplay.PlaySession;

namespace Blackjack.UI
{
    public sealed class GameplayScreenViewModel : IGameplayScreenViewModel
    {
        private readonly UIModel _uiModel;
        private readonly PlaySessionModel _playSessionModel;
        private readonly ApplicationModel _applicationModel;
        
        private readonly ReactiveProperty<bool> _isTakeCardButtonEnabled = new();
        private readonly ReactiveProperty<bool> _isStandButtonEnabled = new();
        private readonly ReactiveProperty<bool> _isLeaveButtonEnabled = new();
        
        IReadOnlyReactiveProperty<bool> IGameplayScreenViewModel.IsTakeCardButtonEnabled => _isTakeCardButtonEnabled;
        IReadOnlyReactiveProperty<bool> IGameplayScreenViewModel.IsStandButtonEnabled => _isStandButtonEnabled;
        IReadOnlyReactiveProperty<bool> IGameplayScreenViewModel.IsLeaveButtonEnabled => _isLeaveButtonEnabled;

        public GameplayScreenViewModel(UIModel uiModel, PlaySessionModel playSessionModel, ApplicationModel applicationModel)
        {
            _uiModel = uiModel;
            _playSessionModel = playSessionModel;
            _applicationModel = applicationModel;

            _playSessionModel.UserTurnAvailable += OnUserTurnAvailable;
            _playSessionModel.RoundFinished += OnRoundFinished;

            _isLeaveButtonEnabled.Value = true;
        }

        public void Dispose()
        {
            _playSessionModel.UserTurnAvailable -= OnUserTurnAvailable;
            _playSessionModel.RoundFinished -= OnRoundFinished;
            
            _isTakeCardButtonEnabled.Dispose();
            _isStandButtonEnabled.Dispose();
            _isLeaveButtonEnabled.Dispose();
        }

        private void OnRoundFinished(RoundResultDto roundResult)
        {
            _uiModel.HideGameplayScreen();
            _uiModel.ShowResultsScreen(new ResultsScreenViewModel(_uiModel, _playSessionModel, _applicationModel, roundResult));
            
            EnableActionButtons(false);
            _isLeaveButtonEnabled.Value = false;
        }

        public void HandleTakeCardButtonClick()
        {
            _playSessionModel.TakeCard();
        }

        public void HandleStandButtonClick()
        {
            _playSessionModel.Stand();
        }

        public void HandleLeaveButtonClick()
        {
            _playSessionModel.FinishSession();
            _uiModel.HideGameplayScreen();
            _uiModel.ShowMenuScreen(new MenuScreenViewModel(_uiModel, _playSessionModel, _applicationModel));
        }
        
        private void OnUserTurnAvailable(bool isTurnAvailable)
        {
            EnableActionButtons(isTurnAvailable);
        }

        private void EnableActionButtons(bool isTurnAvailable)
        {
            _isStandButtonEnabled.Value = isTurnAvailable;
            _isTakeCardButtonEnabled.Value = isTurnAvailable;
        }
    }
}
