using System.Collections.Generic;
using Blackjack.Domain.Round.Events;
using Blackjack.Domain.Round.Rules;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round.Validation
{
    public sealed class RoundCommandValidator
    {
        private readonly HashSet<PlayerId> _reusablePlayerIdsHashSet = new();
        
        public RoundCommandResult ValidateStartRound(
            IReadOnlyList<PlayerId> playerIds,
            RoundState roundState,
            IGameRules gameRules)
        {
            if (playerIds == null)
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidPlayerCount,
                        "Players collection is not provided."));
            }

            if (playerIds.Count == 0)
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidPlayerCount,
                        "Players collection is empty."));
            }
            
            if (playerIds.Count < gameRules.MinPlayersCount ||
                playerIds.Count > gameRules.MaxPlayersCount)
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidPlayerCount,
                        $"Player count must be between {gameRules.MinPlayersCount} and {gameRules.MaxPlayersCount}."));
            }

            if (roundState is { RoundStatus: RoundStatus.Playing })
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidRoundStatus,
                        "Can't start a new round while current round is playing."));
            }

            _reusablePlayerIdsHashSet.Clear();
            
            for (var i = 0; i < playerIds.Count; i++)
            {
                var playerId = playerIds[i];

                if (playerId.IsEmpty)
                {
                    return new RoundCommandResult(
                        RoundCommandStatus.Failure,
                        roundState,
                        RoundEvents.Empty,
                        new RoundError(RoundErrorType.InvalidPlayerId,
                            $"Player id at index {i} should not be empty."));
                }

                if (!_reusablePlayerIdsHashSet.Add(playerId))
                {
                    return new RoundCommandResult(
                        RoundCommandStatus.Failure,
                        roundState,
                        RoundEvents.Empty,
                        new RoundError(RoundErrorType.InvalidPlayerId,
                            "Player id should be unique."));
                }
            }

            return new RoundCommandResult(
                RoundCommandStatus.Success,
                roundState,
                RoundEvents.Empty);
        }

        public RoundCommandResult ValidatePlayerCommand(
            PlayerCommand command,
            RoundState roundState,
            out PlayerState playerState)
        {
            playerState = null;

            if (!IsValidPlayerAction(command.PlayerAction))
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidPlayerAction,
                        $"Player action {command.PlayerAction} is not supported."));
            }

            if (roundState == null)
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState:null,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidRoundState,
                        "Can't handle player command while round state is not created."));
            }
            
            if (roundState.RoundStatus != RoundStatus.Playing)
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidRoundStatus,
                        "Can't handle player command while round is not playing."));
            }

            if (!roundState.TryGetPlayer(command.PlayerId, out playerState))
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.InvalidPlayerId,
                        $"Player {command.PlayerId} doesn't exist in the round state."));
            }

            if (!roundState.CurrentPlayerId.Equals(command.PlayerId))
            {
                return new RoundCommandResult(
                    RoundCommandStatus.Failure,
                    roundState,
                    RoundEvents.Empty,
                    new RoundError(RoundErrorType.WrongTurn,
                        $"Player {command.PlayerId} doesn't have a right to make a turn."));
            }

            return new RoundCommandResult(
                RoundCommandStatus.Success,
                roundState,
                RoundEvents.Empty);
        }

        private static bool IsValidPlayerAction(PlayerAction playerAction)
        {
            switch (playerAction)
            {
                case PlayerAction.TakeCard:
                case PlayerAction.Stand:
                    return true;
                default:
                    return false;
            }
        }
    }
}
