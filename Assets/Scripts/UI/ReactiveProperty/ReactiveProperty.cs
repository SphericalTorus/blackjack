using System;
using System.Collections.Generic;

namespace Blackjack.UI
{
    /// <summary>
    /// This is a naive implementation of reactive property.
    /// </summary>
    public sealed class ReactiveProperty<T> : IReadOnlyReactiveProperty<T> where T : IComparable
    {
        private readonly List<Action<T>> _listeners = new();
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                var valueChanged = !EqualityComparer<T>.Default.Equals(_value, value);
                _value = value;

                if (valueChanged)
                {
                    NotifyListeners();
                }
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            Value = initialValue;
        }
        
        public void Dispose()
        {
            _listeners.Clear();
        }

        public void Subscribe(Action<T> listener)
        {
            _listeners.Add(listener);
            listener?.Invoke(_value);
        }

        private void NotifyListeners()
        {
            foreach (var listener in _listeners)
            {
                listener?.Invoke(_value);
            }
        }
    }
}
