using System;
using Blackjack.Domain.Cards;
using UnityEngine;

namespace Blackjack.Configs.Sprites
{
    [Serializable]
    public sealed class CardSpriteConfig
    {
        [SerializeField]
        private Rank _rank;
        [SerializeField]
        private Suit _suit;
        [SerializeField]
        private Sprite _sprite;

        public Rank Rank => _rank;
        public Suit Suit => _suit;
        public Sprite Sprite => _sprite;
    }
}