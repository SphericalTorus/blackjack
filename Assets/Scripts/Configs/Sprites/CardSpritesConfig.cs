using System.Collections.Generic;
using Blackjack.Domain.Cards;
using UnityEngine;

namespace Blackjack.Configs.Sprites
{
    [CreateAssetMenu(fileName = "CardSprites", menuName = "Blackjack/Card Sprites Config")]
    public sealed class CardSpritesConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<CardSpriteConfig> _spriteConfigs;

        private readonly Dictionary<(Rank, Suit), Sprite> _spritesDict = new();
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _spritesDict.Clear();

            foreach (var spriteConfig in _spriteConfigs)
            {
                if (!_spritesDict.TryAdd((spriteConfig.Rank, spriteConfig.Suit), spriteConfig.Sprite))
                {
                    Debug.LogError($"Duplicated sprite config for card {spriteConfig.Rank} {spriteConfig.Suit}.");
                }
            }
        }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize() {}

        public bool TryGetSprite(Rank rank, Suit suit, out Sprite sprite)
        {
            if (!_spritesDict.TryGetValue((rank, suit), out sprite))
            {
                Debug.LogError($"Sprite not found for card {rank} {suit}.");
            }

            return sprite;
        }
    }
}