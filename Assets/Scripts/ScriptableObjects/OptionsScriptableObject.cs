using UnityEngine;

namespace ArenaFPS.Scripts
{
    [CreateAssetMenu]
    public class OptionsScriptableObject : ScriptableObject
    {
        [Range(0.01f, 10f)]
        public float MouseSensivity = 1f;
    }
}
