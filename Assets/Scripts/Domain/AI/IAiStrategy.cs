using Blackjack.Domain.Cards;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.AI
{
    public interface IAiStrategy
    {
        PlayerAction DecideAction(Hand aiHand, Hand opponentHand);
    }
}
