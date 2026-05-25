using Blackjack.Domain.Cards;
using UnityEngine;

namespace Blackjack.Gameplay.Presentation
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public void Init(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}