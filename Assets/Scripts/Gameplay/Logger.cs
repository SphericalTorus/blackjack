using UnityEngine;
using ILogger = Blackjack.Domain.Tools.ILogger;

namespace Blackjack.Gameplay
{
    public sealed class Logger : ILogger
    {
        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}