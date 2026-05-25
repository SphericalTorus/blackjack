using System;
using System.Collections.Generic;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Player;

namespace Blackjack.Domain.Round
{
    public sealed class RoundState
    {
        private readonly List<PlayerState> _players;
        private readonly Dictionary<PlayerId, int> _playerIndexesById;
        private int _currentPlayerIndex;

        public IReadOnlyList<PlayerState> Players => _players;
        public Deck Deck { get; }
        public PlayerId CurrentPlayerId => _players[_currentPlayerIndex].PlayerId;
        public RoundStatus RoundStatus { get; private set; }
        public RoundResult? RoundResult { get; private set; }

        public RoundState(List<PlayerState> players, Deck deck, PlayerId firstTurnPlayerId)
        {
            _players = players;
            _playerIndexesById = new Dictionary<PlayerId, int>(players.Count);
            var firstTurnPlayerIndex = -1;

            for (var i = 0; i < players.Count; i++)
            {
                var playerState = players[i];
                _playerIndexesById.Add(playerState.PlayerId, i);

                if (playerState.PlayerId == firstTurnPlayerId)
                {
                    firstTurnPlayerIndex = i;
                }
            }
            
            _currentPlayerIndex = firstTurnPlayerIndex;
            Deck = deck;
            RoundStatus = RoundStatus.Playing;
        }

        public bool TryGetPlayer(PlayerId playerId, out PlayerState playerState)
        {
            if (playerId.IsEmpty)
            {
                playerState = null;
                return false;
            }

            if (_playerIndexesById.TryGetValue(playerId, out var playerIndex))
            {
                playerState = _players[playerIndex];
                return true;
            }

            playerState = null;
            return false;
        }

        public void MoveToNextPlayer(Func<PlayerState, bool> canTakeTurn)
        {
            for (var i = 0; i < _players.Count; i++)
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

                if (canTakeTurn(_players[_currentPlayerIndex]))
                {
                    return;
                }
            }
        }

        public void SetRoundResult(RoundResult roundResult)
        {
            RoundResult = roundResult;
        }
        
        public void SetRoundStatus(RoundStatus roundStatus)
        {
            RoundStatus = roundStatus;
        }
    }
}
