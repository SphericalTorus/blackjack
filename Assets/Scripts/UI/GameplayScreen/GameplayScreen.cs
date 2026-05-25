using UnityEngine;
using UnityEngine.UI;

namespace Blackjack.UI
{
    public sealed class GameplayScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _takeCardButton;
        [SerializeField]
        private Button _standButton;
        [SerializeField]
        private Button _leaveButton;

        private IGameplayScreenViewModel _viewModel;
        
        private void Awake()
        {
            _takeCardButton.onClick.AddListener(OnTakeCardButtonClick);
            _standButton.onClick.AddListener(OnStandButtonClick);
            _leaveButton.onClick.AddListener(OnLeaveButtonClick);
        }

        private void OnDestroy()
        {
            _takeCardButton.onClick.RemoveListener(OnTakeCardButtonClick);
            _standButton.onClick.RemoveListener(OnStandButtonClick);
            _leaveButton.onClick.RemoveListener(OnLeaveButtonClick);

            _viewModel?.Dispose();
        }

        public void Init(IGameplayScreenViewModel viewModel)
        {
            _viewModel?.Dispose();
            _viewModel = viewModel;
            
            _viewModel.IsStandButtonEnabled.Subscribe(_standButton.gameObject.SetActive);
            _viewModel.IsTakeCardButtonEnabled.Subscribe(_takeCardButton.gameObject.SetActive);
            _viewModel.IsLeaveButtonEnabled.Subscribe(_leaveButton.gameObject.SetActive);
        }

        public void OnHide()
        {
            _viewModel?.Dispose();
        }

        private void OnLeaveButtonClick()
        {
            _viewModel.HandleLeaveButtonClick();
        }

        private void OnStandButtonClick()
        {
            _viewModel.HandleStandButtonClick();
        }

        private void OnTakeCardButtonClick()
        {
            _viewModel.HandleTakeCardButtonClick();
        }
    }
}