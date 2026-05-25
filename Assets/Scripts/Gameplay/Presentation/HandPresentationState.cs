using System.Collections.Generic;
using Blackjack.Domain.Cards;

namespace Blackjack.Gameplay.Presentation
{
    public sealed class HandPresentationState
    {
        public readonly HandView HandView;
        public readonly List<Card> Cards = new();
        public readonly List<CardView> CardViews = new();

        public HandPresentationState(HandView handView)
        {
            HandView = handView;
        }
    }
}