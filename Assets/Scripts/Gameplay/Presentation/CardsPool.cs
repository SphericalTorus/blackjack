using System;
using Blackjack.Domain.Cards;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Blackjack.Gameplay.Presentation
{
    public sealed class CardsPool : IDisposable
    {
        private readonly CardView _cardViewPrototype;
        private readonly ObjectPool<CardView> _cardsPool;
        
        public CardsPool(CardView cardViewPrototype)
        {
            _cardViewPrototype = cardViewPrototype;

            _cardsPool = new ObjectPool<CardView>(
                createFunc: CreateCard,
                actionOnGet: OnGetCard,
                actionOnRelease: OnReleaseCard,
                actionOnDestroy: OnDestroyCard,
                collectionCheck: true,
                defaultCapacity: Card.UniqueCardsCount,
                maxSize: Card.UniqueCardsCount);

            Prewarm();
        }

        public void Dispose()
        {
            _cardsPool.Dispose();
        }

        public CardView Get()
        {
            return _cardsPool.Get();
        }

        public void Release(CardView cardView)
        {
            _cardsPool.Release(cardView);
        }
        
        private void OnDestroyCard(CardView cardView)
        {
            if (cardView != null)
            {
                Object.Destroy(cardView.gameObject);
            }
        }

        private void OnReleaseCard(CardView cardView)
        {
            if (cardView != null && cardView.gameObject != null)
            {
                cardView.gameObject.SetActive(false);
            }
        }

        private void OnGetCard(CardView cardView)
        {
            cardView.gameObject.SetActive(true);
        }

        private CardView CreateCard()
        {
            return Object.Instantiate(_cardViewPrototype);
        }

        private void Prewarm()
        {
            for (var i = 0; i < Card.UniqueCardsCount; i++)
            {
                var cardView = _cardsPool.Get();
                _cardsPool.Release(cardView);
            }
        }
    }
}
