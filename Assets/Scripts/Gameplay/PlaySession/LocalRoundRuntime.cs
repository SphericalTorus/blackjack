using System;
using System.Collections.Generic;
using Blackjack.Domain.Round.Events;
using Blackjack.Domain.Round;
using Blackjack.Domain.Player;

namespace Blackjack.Gameplay.PlaySession
{
    public sealed class LocalRoundRuntime : IRoundRuntime
    {
        public event Action<RoundEvents> RoundEventsReceived;
        public event Action<RoundState> RoundStateReceived;
        
        private readonly RoundEngine _roundEngine;
        private readonly Logger _logger;
        private RoundState _roundState;

        public RoundState RoundState => _roundState;

        public LocalRoundRuntime(RoundEngine roundEngine, Logger logger)
        {
            _roundEngine = roundEngine;
            _logger = logger;
        }
        
        public bool StartRound(IReadOnlyList<PlayerId> playerIds)
        {
            var commandResult = _roundEngine.StartRound(playerIds);

            if (commandResult.Status != RoundCommandStatus.Success)
            {
                _logger.LogError($"Failed to start round. {commandResult.Status} {commandResult.Error}");
                return false;
            }
            
            ProcessCommandResult(commandResult);
            return true;
        }

        public void ExecutePlayerCommand(PlayerId playerId, PlayerAction playerAction)
        {
            var commandResult = _roundEngine.ExecutePlayerCommand(new PlayerCommand(playerId, playerAction));
            
            if (commandResult.Status != RoundCommandStatus.Success)
            {
                _logger.LogError($"Failed to execute player {playerId} command {playerAction}. {commandResult.Status} {commandResult.Error}");
                return;
            }

            ProcessCommandResult(commandResult);
        }

        private void ProcessCommandResult(RoundCommandResult commandResult)
        {
            _roundState = commandResult.RoundState;
            
            RoundStateReceived?.Invoke(commandResult.RoundState);
            RoundEventsReceived?.Invoke(commandResult.Events);
        }
    }
}
