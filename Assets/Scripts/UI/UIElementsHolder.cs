using UnityEngine;

namespace Blackjack.UI
{
    public sealed class UIElementsHolder : MonoBehaviour, IUIElementsHolder
    {
        [SerializeField]
        private MenuScreen _menuScreen;
        [SerializeField]
        private GameplayScreen _gameplayScreen;
        [SerializeField]
        private ResultsScreen _resultsScreen;

        MenuScreen IUIElementsHolder.MenuScreen => _menuScreen;
        GameplayScreen IUIElementsHolder.GameplayScreen => _gameplayScreen;
        ResultsScreen IUIElementsHolder.ResultsScreen => _resultsScreen;
    }
}