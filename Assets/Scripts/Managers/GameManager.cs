using DG.Tweening;
using UnityEngine;

namespace ArenaFPS.Scripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public OptionsScriptableObject Options;
        public GameObject Player;

        public InputSystemActions InputActions { get; private set; }
        public int CurrentLevel = 1;

        private void Awake()
        {
            InputActions = new InputSystemActions();
            InputActions.Player.Enable();
            DOTween.Init();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            CurrentLevel = 1;
        }
    }
}
