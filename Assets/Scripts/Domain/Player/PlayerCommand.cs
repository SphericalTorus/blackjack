namespace Blackjack.Domain.Player
{
    public readonly struct PlayerCommand
    {
        public PlayerId PlayerId { get; }
        public PlayerAction PlayerAction { get; }

        public PlayerCommand(PlayerId playerId, PlayerAction playerAction)
        {
            PlayerId = playerId;
            PlayerAction = playerAction;
        }
    }
}
