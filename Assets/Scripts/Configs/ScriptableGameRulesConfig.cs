using Blackjack.Domain.Round.Rules;
using UnityEngine;

namespace Blackjack.Configs
{
    [CreateAssetMenu(fileName = "BlackjackRulesConfig", menuName = "Blackjack/Game Rules Config")]
    public sealed class ScriptableGameRulesConfig : ScriptableObject, IGameRulesConfig
    {
        [SerializeField]
        private string _rulesName = "Default Blackjack";
        [SerializeField]
        private int _minPlayerCount = 2;
        [SerializeField]
        private int _maxPlayerCount = 2;
        [SerializeField]
        private int _targetScore = 21;
        [SerializeField]
        private int _initialCardsPerPlayer = 2;
        [SerializeField]
        private int _faceCardValue = 10;
        [SerializeField]
        private int _aceLowValue = 1;
        [SerializeField]
        private int _aceHighValue = 11;

        public string Name => _rulesName;
        public int MinPlayerCount => _minPlayerCount;
        public int MaxPlayerCount => _maxPlayerCount;
        public int TargetScore => _targetScore;
        public int InitialCardsPerPlayer => _initialCardsPerPlayer;
        public int FaceCardValue => _faceCardValue;
        public int AceLowValue => _aceLowValue;
        public int AceHighValue => _aceHighValue;
    }
}
