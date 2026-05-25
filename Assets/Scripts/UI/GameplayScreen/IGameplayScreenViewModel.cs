using System;

namespace Blackjack.UI
{
    public interface IGameplayScreenViewModel : IDisposable
    {
        IReadOnlyReactiveProperty<bool> IsTakeCardButtonEnabled { get; }
        IReadOnlyReactiveProperty<bool> IsStandButtonEnabled { get; }
        IReadOnlyReactiveProperty<bool> IsLeaveButtonEnabled { get; }
        
        void HandleTakeCardButtonClick();
        void HandleStandButtonClick();
        void HandleLeaveButtonClick();
    }
}