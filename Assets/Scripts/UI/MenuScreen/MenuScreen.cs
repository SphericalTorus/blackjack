using UnityEngine;
using UnityEngine.UI;

namespace Blackjack.UI
{
    public sealed class MenuScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private Button _exitButton;

        private IMenuScreenViewModel _viewModel;
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        public void Init(IMenuScreenViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnPlayButtonClick()
        {
            _viewModel.HandlePlayButtonClick();
        }
        
        private void OnExitButtonClick()
        {
            _viewModel.HandleExitButtonClick();
        }
    }
}