using System;
using System.Collections.Generic;
using Blackjack.Configs;
using Blackjack.Configs.Sprites;
using Blackjack.Domain.Cards;
using Blackjack.Domain.Round;
using Blackjack.Domain.Round.Rules;
using Blackjack.Gameplay;
using Blackjack.Gameplay.Presentation;
using Blackjack.Gameplay.PlaySession;
using Blackjack.UI;
using UnityEngine;

namespace Blackjack.DI
{
    public sealed class GameInstaller : MonoBehaviour
    {
        [SerializeField]
        private UIElementsHolder _uiElementsHolder;
        [SerializeField]
        private ScriptableGameRulesConfig _gameRulesConfig;
        [SerializeField]
        private CardSpritesConfig _cardSpritesConfig;
        [SerializeField]
        private SceneContext _sceneContext;
        [SerializeField]
        private CardView _cardViewPrototype;

        private readonly List<IDisposable> _disposables = new();
        private PlaySessionModel _playSessionModel;
        private TableModel _tableModel;

        private bool _initialized;

        private void Awake()
        {
            var logger = new Gameplay.Logger();
            var random = new DefaultRandom();
            
            var gameRulesConfigValidator = new GameRulesConfigValidator(logger);

            if (!gameRulesConfigValidator.Validate(_gameRulesConfig))
            {
                logger.LogError("Config validation failed. Game can't be started.");
                return;
            }
            
            var defaultGameRules = new DefaultGameRules(_gameRulesConfig);
            var cardShuffler = new CardShuffler(random);
            var deckFactory = new DeckFactory(cardShuffler);
            var roundEngine = new RoundEngine(defaultGameRules, deckFactory, random);
            
            _playSessionModel = new PlaySessionModel(roundEngine, defaultGameRules, logger);
            _disposables.Add(_playSessionModel);

            var applicationModel = new ApplicationModel();
            
            var uiModel = new UIModel(_uiElementsHolder);
            uiModel.ShowMenuScreen(new MenuScreenViewModel(uiModel, _playSessionModel, applicationModel));

            _tableModel = new TableModel(_playSessionModel, _cardSpritesConfig, logger, _sceneContext, _cardViewPrototype);
            _disposables.Add(_tableModel);

            _initialized = true;
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }
            
            _playSessionModel.Update(Time.deltaTime);
            _tableModel.Update(Time.deltaTime);
        }
    }
}
