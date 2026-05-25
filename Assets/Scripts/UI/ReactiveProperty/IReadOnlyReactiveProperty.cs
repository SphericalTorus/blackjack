using System;

namespace Blackjack.UI
{
    public interface IReadOnlyReactiveProperty<out T> : IDisposable
    {
        T Value { get; }

        void Subscribe(Action<T> listener);
    }
}