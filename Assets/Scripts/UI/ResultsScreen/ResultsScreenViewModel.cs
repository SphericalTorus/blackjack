using System.Text;
using Blackjack.Gameplay.DTO;
using Blackjack.Gameplay.PlaySession;

namespace Blackjack.UI
{
    public sealed class ResultsScreenViewModel : IResultsScreenViewModel
    {
        private readonly static StringBuilder StringBuilder = new();
        
        private readonly UIModel _uiModel;
        private readonly PlaySessionModel _playSessionModel;
        private readonly ApplicationModel _applicationModel;

        private readonly string _resultText;

        string IResultsScreenViewModel.ResultText => _resultText;

        public ResultsScreenViewModel(
            UIModel uiModel, 
            PlaySessionModel playSessionModel, 
            ApplicationModel applicationModel, 
            RoundResultDto roundResult)
        {
            _uiModel = uiModel;
            _playSessionModel = playSessionModel;
            _applicationModel = applicationModel;

            _resultText = BuildResultText(roundResult);
        }

        public void HandleToMenuButtonClick()
        {
            _playSessionModel.FinishSession();
            _uiModel.HideResultsScreen();
            _uiModel.ShowMenuScreen(new MenuScreenViewModel(_uiModel, _playSessionModel, _applicationModel));
        }

        public void HandlePlayAgainButtonClick()
        {
            _playSessionModel.FinishSession();
            _uiModel.HideResultsScreen();
            _uiModel.ShowGameplayScreen(new GameplayScreenViewModel(_uiModel, _playSessionModel, _applicationModel));

            if (!_playSessionModel.StartSession())
            {
                _uiModel.HideGameplayScreen();
                _uiModel.ShowMenuScreen(new MenuScreenViewModel(_uiModel, _playSessionModel, _applicationModel));
            }
        }

        private string BuildResultText(RoundResultDto roundResult)
        {
            StringBuilder.Clear();
            var player = roundResult.PlayerResults.Find(playerResult => playerResult.PlayerId == _playSessionModel.PlayerId);
            var opponent = roundResult.PlayerResults.Find(playerResult => playerResult.PlayerId != _playSessionModel.PlayerId);
            
            if (roundResult.IsDraw)
            {
                StringBuilder.AppendLine("Draw!");
            } 
            else if (roundResult.Winner != null)
            {
                StringBuilder.AppendLine(roundResult.Winner.PlayerId == _playSessionModel.PlayerId
                    ? "Win!"
                    : player is { IsBust: true } ? "Bust!" : "Lose!");
            }

            if (player != null && opponent != null)
            {
                StringBuilder.Append("Your score: ");
                StringBuilder.AppendLine(player.ScoreValue.ToString());
                StringBuilder.Append("Opponent's score: ");
                StringBuilder.Append(opponent.ScoreValue.ToString());
            }

            return StringBuilder.ToString();
        }
    }
}
