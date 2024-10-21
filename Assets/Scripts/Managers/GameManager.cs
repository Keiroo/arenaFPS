using DG.Tweening;
using UnityEngine;

namespace ArenaFPS.Scripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public OptionsScriptableObject Options;
        public InputSystemActions InputActions;

        private void Awake()
        {
            InputActions = new InputSystemActions();
            InputActions.Player.Enable();
            DOTween.Init();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
