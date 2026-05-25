using System;
using Blackjack.Domain.AI;
using Blackjack.Domain.Round.Events;
using Blackjack.Domain.Round;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;
using Blackjack.Gameplay.AI;
using Blackjack.Gameplay.DTO;

namespace Blackjack.Gameplay.PlaySession
{
    public sealed class PlaySessionModel : IDisposable
    {
        public event Action<bool> UserTurnAvailable;
        public event Action<RoundResultDto> RoundFinished;
        
        private readonly RoundEngine _roundEngine;
        private readonly Logger _logger;
        private readonly DtoMapper _dtoMapper;
        private readonly IAiStrategy _aiStrategy;
        
        private readonly PlayerId _playerId = new("human");
        private readonly PlayerId _aiPlayerId = new("ai");
        private readonly PlayerId[] _sessionPlayerIds;
        
        private IRoundRuntime _roundRuntime;
        private SimpleAiController _aiController;
        
        public IRoundRuntime RoundRuntime => _roundRuntime;
        public PlayerId PlayerId => _playerId;

        public PlaySessionModel(RoundEngine roundEngine, IGameRules gameRules, Logger logger)
        {
            _roundEngine = roundEngine;
            _logger = logger;

            _dtoMapper = new DtoMapper();
            _aiStrategy = new SimpleAiStrategy(gameRules);
            _sessionPlayerIds = new[] { _playerId, _aiPlayerId };
        }

        public void Dispose()
        {
            FinishSession();
        }
        
        public void Update(float deltaTime)
        {
            _aiController?.Update(deltaTime);
        }

        public bool StartSession()
        {
            FinishSession();
            
            _roundRuntime = new LocalRoundRuntime(_roundEngine, _logger);
            _roundRuntime.RoundEventsReceived += OnRoundEventsReceived;
            
            _aiController = new SimpleAiController(
                _roundRuntime,
                _aiStrategy,
                _aiPlayerId,
                _playerId);
            
            var roundStarted = _roundRuntime.StartRound(_sessionPlayerIds);

            if (!roundStarted)
            {
                FinishSession();
            }

            return roundStarted;
        }

        public void FinishSession()
        {
            if (_roundRuntime != null)
            {
                _roundRuntime.RoundEventsReceived -= OnRoundEventsReceived;
            }
            
            _aiController = null;
            _roundRuntime = null;
            _roundEngine.ResetRound();
        }

        public void TakeCard()
        {
            _roundRuntime.ExecutePlayerCommand(_playerId, PlayerAction.TakeCard);
        }

        public void Stand()
        {
            _roundRuntime.ExecutePlayerCommand(_playerId, PlayerAction.Stand);
        }
        
        private void OnRoundEventsReceived(RoundEvents events)
        {
            if (events.HasCurrentPlayerChanged)
            {
                if (events.CurrentPlayerId == _playerId)
                {
                    UserTurnAvailable?.Invoke(true);
                }
                else if (events.CurrentPlayerId == _aiPlayerId)
                {
                    UserTurnAvailable?.Invoke(false);
                    _aiController.QueueTurn();
                }
            }

            if (events.HasRoundStatusChanged &&
                events.RoundStatus == RoundStatus.Finished)
            {
                UserTurnAvailable?.Invoke(false);
                
                var roundResult = _roundRuntime.RoundState.RoundResult;

                if (roundResult.HasValue)
                {
                    var roundResultDto = _dtoMapper.Map(roundResult.Value);
                    RoundFinished?.Invoke(roundResultDto);
                }
            }
        }
    }
}
