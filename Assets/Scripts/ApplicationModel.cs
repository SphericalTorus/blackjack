#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Blackjack
{
    public sealed class ApplicationModel
    {
        public void ExitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}