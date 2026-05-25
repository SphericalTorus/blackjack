using System;
using System.Collections.Generic;
using Blackjack.Domain.Round.Events;
using Blackjack.Domain.Round;
using Blackjack.Domain.Player;

namespace Blackjack.Gameplay.PlaySession
{
    public interface IRoundRuntime
    {
        event Action<RoundEvents> RoundEventsReceived;
        event Action<RoundState> RoundStateReceived;
        
        RoundState RoundState { get; }
        
        bool StartRound(IReadOnlyList<PlayerId> playerIds);
        void ExecutePlayerCommand(PlayerId playerId, PlayerAction playerAction);
    }
}
