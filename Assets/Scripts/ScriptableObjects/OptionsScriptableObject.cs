using UnityEngine;

namespace ArenaFPS.Scripts
{
    [CreateAssetMenu]
    public class OptionsScriptableObject : ScriptableObject
    {
        [Range(0.01f, 1f)]
        public float LookSensitivity = 0.5f;
    }
}
