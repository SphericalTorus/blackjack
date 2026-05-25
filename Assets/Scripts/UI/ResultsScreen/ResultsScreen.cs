using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blackjack.UI
{
    public sealed class ResultsScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _playAgainButton;
        [SerializeField]
        private Button _toMenuButton;
        [SerializeField]
        private TextMeshProUGUI _resultsText;

        private IResultsScreenViewModel _viewModel;
        
        private void Awake()
        {
            _playAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
            _toMenuButton.onClick.AddListener(OnToMenuButtonClick);
        }

        private void OnDestroy()
        {
            _playAgainButton.onClick.RemoveListener(OnPlayAgainButtonClick);
            _toMenuButton.onClick.RemoveListener(OnToMenuButtonClick);
        }

        public void Init(IResultsScreenViewModel viewModel)
        {
            _viewModel = viewModel;
            _resultsText.text = viewModel.ResultText;
        }
        
        private void OnPlayAgainButtonClick()
        {
            _viewModel.HandlePlayAgainButtonClick();
        }
        
        private void OnToMenuButtonClick()
        {
            _viewModel.HandleToMenuButtonClick();
        }
    }
}
