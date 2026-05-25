namespace Blackjack.Domain.Tools
{
    public interface IRandom
    {
        double NextDouble();
        int Next(int maxValue);
    }
}