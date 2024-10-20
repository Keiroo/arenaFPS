#if UNITY_EDITOR
using System.Diagnostics;
#endif

namespace ArenaFPS.Scripts
{
    public static class DebugLogger
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            var stackTrace = new StackTrace();
            var className = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;
            UnityEngine.Debug.Log($"{className}: {message}");
#endif
        }
    }
}
