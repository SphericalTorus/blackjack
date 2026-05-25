using System;
using System.Collections.Generic;
using Blackjack.Configs.Sprites;
using Blackjack.DI;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round;
using Blackjack.Gameplay.PlaySession;
using UnityEngine;
using ILogger = Blackjack.Domain.Tools.ILogger;

namespace Blackjack.Gameplay.Presentation
{
    public sealed class TableModel : IDisposable
    {
        private const int BottomPlayerIndex = 0;
        private const int TopPlayerIndex = 1;
        private const float CardMoveLerpSpeed = 6.67f;

        private readonly PlaySessionModel _playSessionModel;
        private readonly CardSpritesConfig _cardSpritesConfig;
        private readonly ILogger _logger;
        private readonly SceneContext _sceneContext;
        private readonly CardsPool _cardsPool;
        private readonly HandPresentationState[] _hands;

        private IRoundRuntime _observedRoundRuntime;

        public TableModel(
            PlaySessionModel playSessionModel,
            CardSpritesConfig cardSpritesConfig,
            ILogger logger,
            SceneContext sceneContext,
            CardView cardViewPrototype)
        {
            _playSessionModel = playSessionModel;
            _cardSpritesConfig = cardSpritesConfig;
            _logger = logger;
            _sceneContext = sceneContext;

            _cardsPool = new CardsPool(cardViewPrototype);
            _hands = new[]
            {
                new HandPresentationState(_sceneContext.BottomPlayerHandView),
                new HandPresentationState(_sceneContext.TopPlayerHandView),
            };

            var cardWidth = GetCardWidth(cardViewPrototype);
            _sceneContext.BottomPlayerHandView.Init(cardWidth);
            _sceneContext.TopPlayerHandView.Init(cardWidth);
        }

        public void Dispose()
        {
            StopObservingRoundRuntime();
            ReleaseAllCards();
            _cardsPool.Dispose();
        }

        public void Update(float deltaTime)
        {
            RefreshObservedRoundRuntime();
            MoveCards(deltaTime);
        }

        private void RefreshObservedRoundRuntime()
        {
            var roundRuntime = _playSessionModel.RoundRuntime;

            if (ReferenceEquals(_observedRoundRuntime, roundRuntime))
            {
                return;
            }

            StopObservingRoundRuntime();
            ReleaseAllCards();

            _observedRoundRuntime = roundRuntime;

            if (_observedRoundRuntime == null)
            {
                return;
            }

            _observedRoundRuntime.RoundStateReceived += OnRoundStateReceived;

            if (_observedRoundRuntime.RoundState != null)
            {
                OnRoundStateReceived(_observedRoundRuntime.RoundState);
            }
        }

        private void StopObservingRoundRuntime()
        {
            if (_observedRoundRuntime == null)
            {
                return;
            }

            _observedRoundRuntime.RoundStateReceived -= OnRoundStateReceived;
            _observedRoundRuntime = null;
        }

        private void OnRoundStateReceived(RoundState roundState)
        {
            SyncHand(_hands[BottomPlayerIndex], roundState, BottomPlayerIndex);
            SyncHand(_hands[TopPlayerIndex], roundState, TopPlayerIndex);
        }

        private void SyncHand(HandPresentationState handState, RoundState roundState, int playerIndex)
        {
            if (playerIndex >= roundState.Players.Count)
            {
                ReleaseCardsFromIndex(handState, 0);
                handState.HandView.SetCardCount(0);
                return;
            }

            var cards = roundState.Players[playerIndex].Hand.Cards;
            var matchingCardsCount = GetMatchingCardsCount(handState.Cards, cards);

            ReleaseCardsFromIndex(handState, matchingCardsCount);

            for (var i = matchingCardsCount; i < cards.Count; i++)
            {
                AddCard(handState, cards[i], playerIndex, i);
            }

            handState.HandView.SetCardCount(cards.Count);
        }

        private void AddCard(HandPresentationState handState, Card card, int playerIndex, int cardIndex)
        {
            var cardView = _cardsPool.Get();
            cardView.Init(card.Rank, card.Suit);
            cardView.transform.position = _sceneContext.DeckTransform.position;

            cardView.SpriteRenderer.sprite = null;

            if (_cardSpritesConfig.TryGetSprite(card.Rank, card.Suit, out var sprite))
            {
                cardView.SpriteRenderer.sprite = sprite;
            }
            else
            {
                _logger.LogError($"Card {card.Rank} {card.Suit} sprite was not found.");
            }

            cardView.SpriteRenderer.sortingOrder = playerIndex * Card.UniqueCardsCount + cardIndex;

            handState.Cards.Add(card);
            handState.CardViews.Add(cardView);
        }

        private void MoveCards(float deltaTime)
        {
            var lerpFactor = Mathf.Clamp01(deltaTime * CardMoveLerpSpeed);

            foreach (var handState in _hands)
            {
                for (var i = 0; i < handState.CardViews.Count; i++)
                {
                    if (!handState.HandView.TryGetCardPosition(i, out var targetPosition))
                    {
                        continue;
                    }

                    var cardTransform = handState.CardViews[i].transform;
                    cardTransform.position = Vector3.Lerp(
                        cardTransform.position,
                        targetPosition,
                        lerpFactor);
                }
            }
        }

        private void ReleaseAllCards()
        {
            foreach (var handState in _hands)
            {
                ReleaseCardsFromIndex(handState, startIndex:0);
                handState.HandView.SetCardCount(0);
            }
        }

        private void ReleaseCardsFromIndex(HandPresentationState handState, int startIndex)
        {
            for (var i = handState.CardViews.Count - 1; i >= startIndex; i--)
            {
                _cardsPool.Release(handState.CardViews[i]);
                handState.CardViews.RemoveAt(i);
                handState.Cards.RemoveAt(i);
            }
        }

        // Keep the unchanged prefix of card views so only newly drawn cards animate from the deck.
        private int GetMatchingCardsCount(IReadOnlyList<Card> currentCards, IReadOnlyList<Card> nextCards)
        {
            var matchingCardsCount = Mathf.Min(currentCards.Count, nextCards.Count);

            for (var i = 0; i < matchingCardsCount; i++)
            {
                if (!AreSameCards(currentCards[i], nextCards[i]))
                {
                    return i;
                }
            }

            return matchingCardsCount;
        }

        private bool AreSameCards(Card left, Card right)
        {
            return left.Rank == right.Rank && left.Suit == right.Suit;
        }

        private float GetCardWidth(CardView cardView)
        {
            var sprite = cardView.SpriteRenderer.sprite;

            if (sprite == null)
            {
                return 0f;
            }

            return sprite.bounds.size.x * Mathf.Abs(cardView.transform.lossyScale.x);
        }
    }
}
