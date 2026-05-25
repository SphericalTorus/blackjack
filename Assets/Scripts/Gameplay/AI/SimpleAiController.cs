using Blackjack.Domain.AI;
using Blackjack.Domain.Round;
using Blackjack.Domain.Player;
using Blackjack.Gameplay.PlaySession;

namespace Blackjack.Gameplay.AI
{
    public sealed class SimpleAiController
    {
        private const float TurnDelaySeconds = 1f;
        
        private readonly IRoundRuntime _roundRuntime;
        private readonly IAiStrategy _aiStrategy;
        private readonly PlayerId _aiPlayerId;
        private readonly PlayerId _opponentPlayerId;
        private float _turnDelayTimer;
        private bool _isTurnQueued;

        public SimpleAiController(
            IRoundRuntime roundRuntime,
            IAiStrategy aiStrategy,
            PlayerId aiPlayerId,
            PlayerId opponentPlayerId)
        {
            _roundRuntime = roundRuntime;
            _aiStrategy = aiStrategy;
            _aiPlayerId = aiPlayerId;
            _opponentPlayerId = opponentPlayerId;
        }

        public void QueueTurn()
        {
            _turnDelayTimer = 0f;
            _isTurnQueued = true;
        }

        public void Update(float deltaTime)
        {
            if (!_isTurnQueued)
            {
                return;
            }

            _turnDelayTimer += deltaTime;

            if (_turnDelayTimer < TurnDelaySeconds)
            {
                return;
            }

            _isTurnQueued = false;
            TryExecuteTurn();
        }

        private void TryExecuteTurn()
        {
            var roundState = _roundRuntime.RoundState;
            
            if (roundState == null ||
                roundState.RoundStatus != RoundStatus.Playing ||
                roundState.CurrentPlayerId != _aiPlayerId)
            {
                return;
            }
            
            if (!roundState.TryGetPlayer(_aiPlayerId, out var aiPlayer) ||
                !roundState.TryGetPlayer(_opponentPlayerId, out var opponent) ||
                aiPlayer.IsStanding)
            {
                return;
            }

            var action = _aiStrategy.DecideAction(aiPlayer.Hand, opponent.Hand);
            _roundRuntime.ExecutePlayerCommand(_aiPlayerId, action);
        }
    }
}
