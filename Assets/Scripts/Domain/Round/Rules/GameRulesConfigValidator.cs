using Blackjack.Domain.Cards;
using Blackjack.Domain.Tools;

namespace Blackjack.Domain.Round.Rules
{
    public sealed class GameRulesConfigValidator
    {
        private readonly ILogger _logger;

        public GameRulesConfigValidator(ILogger logger)
        {
            _logger = logger;
        }
        
        public bool Validate(IGameRulesConfig config)
        {
            if (string.IsNullOrEmpty(config.Name))
            {
                _logger.LogError("Game rules config should be named.");
                return false;
            }

            if (config.InitialCardsPerPlayer < 0)
            {
                _logger.LogError("InitialCardsPerPlayer should not be negative.");
                return false;
            }
            
            if (config.FaceCardValue <= 0)
            {
                _logger.LogError("FaceCardValue should be more than zero.");
                return false;
            }
            
            if (config.AceLowValue <= 0)
            {
                _logger.LogError("AceLowValue should be more than zero.");
                return false;
            }
            
            if (config.AceLowValue > config.AceHighValue)
            {
                _logger.LogError($"AceLowValue ({config.AceLowValue}) count should be less or equal to AceHighValue ({config.AceHighValue}).");
                return false;
            }
            
            if (config.MinPlayerCount <= 0)
            {
                _logger.LogError($"Invalid min number of of players {config.MinPlayerCount}.");
                return false;
            }

            if (config.MinPlayerCount > config.MaxPlayerCount)
            {
                _logger.LogError($"MinPlayerCount ({config.MinPlayerCount}) count should be less or equal to MaxPlayerCount ({config.MaxPlayerCount}).");
                return false;
            }
            
            var cardsRequired = config.MaxPlayerCount * config.InitialCardsPerPlayer;

            if (cardsRequired > Card.UniqueCardsCount)
            {
                _logger.LogError($"Min number of cards required ({cardsRequired}) is higher than the number of cards in standard deck ({Card.UniqueCardsCount}).");   
                return false;
            }

            return true;
        }
    }
}
