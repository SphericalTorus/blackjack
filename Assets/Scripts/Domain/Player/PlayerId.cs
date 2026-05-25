using System;

namespace Blackjack.Domain.Player
{
    public readonly struct PlayerId : IEquatable<PlayerId>
    {
        private readonly string _value;

        public string Value => _value ?? string.Empty;
        public bool IsEmpty => string.IsNullOrWhiteSpace(_value);

        public PlayerId(string value)
        {
            _value = value;
        }

        public bool Equals(PlayerId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(PlayerId left, PlayerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlayerId left, PlayerId right)
        {
            return !left.Equals(right);
        }
    }
}
