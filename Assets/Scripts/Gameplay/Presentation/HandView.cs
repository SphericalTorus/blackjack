using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Gameplay.Presentation
{
    public sealed class HandView : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _maxWidth = 6f;
        [SerializeField, Min(0f)]
        private float _preferredSpacing = 1.2f;
        [SerializeField]
        private Vector3 _localOffset;

        private readonly List<Vector3> _cardLocalPositions = new();

        private float _cardWidth;
        private int _cardCount;

        public void Init(float cardWidth)
        {
            _cardWidth = cardWidth;
        }

        public void SetCardCount(int cardCount)
        {
            _cardCount = Mathf.Max(0, cardCount);
            RecalculatePositions();
        }
        
        public bool TryGetCardPosition(int cardIndex, out Vector3 position)
        {
            if (!TryGetCardLocalPosition(cardIndex, out var localPosition))
            {
                position = default;
                return false;
            }

            position = transform.TransformPoint(localPosition);
            return true;
        }
        
        private bool TryGetCardLocalPosition(int cardIndex, out Vector3 position)
        {
            if (cardIndex < 0 || cardIndex >= _cardLocalPositions.Count)
            {
                position = default;
                return false;
            }

            position = _cardLocalPositions[cardIndex];
            return true;
        }

        private void OnValidate()
        {
            _maxWidth = Mathf.Max(0f, _maxWidth);
            _cardWidth = Mathf.Clamp(_cardWidth, 0f, _maxWidth);
            _preferredSpacing = Mathf.Max(0f, _preferredSpacing);

            RecalculatePositions();
        }

        private void RecalculatePositions()
        {
            _cardLocalPositions.Clear();

            if (_cardCount == 0)
            {
                return;
            }

            if (_cardCount == 1)
            {
                _cardLocalPositions.Add(_localOffset);
                return;
            }

            var spacing = CalculateSpacing(_cardCount);
            var positionsWidth = spacing * (_cardCount - 1);
            var startX = -positionsWidth * 0.5f;

            for (var i = 0; i < _cardCount; i++)
            {
                _cardLocalPositions.Add(_localOffset + new Vector3(startX + spacing * i, 0f, 0f));
            }
        }

        private float CalculateSpacing(int cardCount)
        {
            var availableSpacingWidth = Mathf.Max(0f, _maxWidth - _cardWidth);
            var maxSpacing = availableSpacingWidth / (cardCount - 1);

            return Mathf.Min(_preferredSpacing, maxSpacing);
        }
    }
}
