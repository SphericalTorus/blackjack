## Project Setup

- Main scene: `Assets/Scenes/MainScene.unity`

## Project Structure

- `Assets/Scripts/Domain`  
  Unity-independent game logic: cards, deck, hand scoring, round engine, rules, validation, player state, and AI strategy interfaces. This assembly has `noEngineReferences` enabled.

- `Assets/Scripts/Gameplay`  
  Unity-facing gameplay layer. It connects the domain round engine to a local play session, AI controller, DTO mapping, table presentation, card views, and hand views.

- `Assets/Scripts/UI`  
  Simple UI screens and view models for menu, gameplay, and results. The UI is intentionally lightweight and mostly acts as a presentation layer over the gameplay model.

- `Assets/Scripts/DI`  
  Manual scene composition. `GameInstaller` is the main entry point for runtime setup.

- `Assets/Scripts/Configs`  
  ScriptableObject configuration for game rules and card sprite mapping.

- `Assets/Tests/EditMode/Domain`  
  Partial Edit Mode test coverage for the domain layer, rules, round engine, and AI strategy.

- `Assets/Scenes`, `Assets/Prefabs`, `Assets/Sprites`  
  Scene, prefab, and visual assets, including the provided card assets.

## Execution Flow

Runtime setup starts in `GameInstaller.Awake()` in `MainScene`.

`GameInstaller` validates the rule configuration, creates the domain services (`DefaultGameRules`, `DeckFactory`, `RoundEngine`), creates the gameplay session model, wires the UI model, and creates the table presentation model.

When a session starts, `PlaySessionModel` creates a `LocalRoundRuntime`, starts a round through `RoundEngine`, and connects the `SimpleAiController`. The domain layer returns immutable round state and round events. The gameplay and UI layers react to those events to update controls, card presentation, and the result screen.

`GameInstaller.Update()` ticks the play session and table presentation. This keeps Unity-specific timing outside of the domain model.

## Main Class Interactions

High-level gameplay flow:

`UI button -> ScreenViewModel -> PlaySessionModel -> LocalRoundRuntime -> RoundEngine -> RoundState/RoundEvents -> UI/TableModel`

- `GameInstaller` is the composition root. It creates domain services, gameplay models, UI models, and presentation models, then updates the session and table models every frame.
- `RoundEngine` owns the round state changes. It validates commands, deals cards, advances the active player, checks bust/end conditions, and returns updated state plus events.
- `PlaySessionModel` is the bridge between UI/gameplay code and the domain engine. It starts and finishes local rounds, forwards human commands, listens to round events, enables/disables the human turn, and publishes round results.
- `LocalRoundRuntime` wraps the domain `RoundEngine` behind `IRoundRuntime`. This keeps the session model independent from the concrete runtime and leaves room for a remote/server-backed implementation later.
- `SimpleAiController` reacts when the current player becomes the AI. After a short delay, it asks `IAiStrategy` for a decision and sends the command through `IRoundRuntime`.
- `GameplayScreenViewModel`, `MenuScreenViewModel`, and `ResultsScreenViewModel` handle screen-level user actions and navigation. They do not contain game rules.
- `TableModel` observes round state updates and synchronizes Unity card views with the domain hands. It is presentation-only and does not affect game decisions.

## Architecture Notes

The domain logic is intentionally detached from Unity to keep it testable and reusable. It is written in a relatively general way, so supporting more than two players should mostly be a matter of adding an appropriate runtime/presentation layer. The current Unity presentation is deliberately limited to exactly two visible players for simplicity.

No external DI framework is used. This was a conscious choice to avoid extra dependencies in a small test project. In a larger production project, I would likely use a DI framework or a more formal composition approach.

The UI system is intentionally primitive. A polished, animated, production-quality UI would require a separate amount of work, so the focus here is on architecture, gameplay logic, and clear boundaries. The current presentation layer is still separated enough to allow animations to be added later.

Test coverage is partial and mainly demonstrates that the domain layer can be tested. More tests would be added in a production scenario, especially around full gameplay flows and UI integration.

## AI and External Support Disclosure

No external gameplay/runtime libraries were added beyond Unity packages.

AI support was used in a limited capacity as a productivity aid, mainly for mechanical refactoring assistance, naming consistency, `sealed` modifier consistency, minor implementation/cleanup suggestions, and README wording. All suggestions were reviewed and adapted before integration. The gameplay logic, architecture decisions, testing, integration, and final review were done by me.
