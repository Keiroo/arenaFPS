using UnityEngine;

namespace ArenaFPS.Scripts
{
    public class DebugEnable : MonoBehaviour
    {
        private void Awake()
        {
            if (!Application.isEditor && Debug.isDebugBuild)
                gameObject.SetActive(false);
        }
    }
}
