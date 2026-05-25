using Blackjack.Gameplay.Presentation;
using UnityEngine;

namespace Blackjack.DI
{
    public sealed class SceneContext : MonoBehaviour
    {
        [SerializeField]
        private Transform _deckTransform;
        [SerializeField]
        private HandView _bottomPlayerHandView;
        [SerializeField]
        private HandView _topPlayerHandView;
        
        public Transform DeckTransform => _deckTransform;
        public HandView BottomPlayerHandView => _bottomPlayerHandView;
        public HandView TopPlayerHandView => _topPlayerHandView;
    }
}