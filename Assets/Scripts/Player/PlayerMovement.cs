using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float MoveSpeed = 10f;
        private CharacterController controller;
        private bool isMoving = false;
        private Vector2 moveRawValue = Vector2.zero;

        private void Awake()
        {
            if (!TryGetComponent(out controller))
                DebugLogger.Log("Controller not found");
        }

        private void Start()
        {
            GameManager.Instance.InputActions.Player.Move.started += OnMoveStart;
            GameManager.Instance.InputActions.Player.Move.canceled += OnMoveCancel;
        }

        private void FixedUpdate()
        {
            if (isMoving)
                Move();
        }

        private void OnMoveStart(InputAction.CallbackContext context)
        {
            isMoving = true;
        }

        private void OnMoveCancel(InputAction.CallbackContext context)
        {
            isMoving = false;
            moveRawValue = Vector2.zero;
        }

        private void Move()
        {
            moveRawValue = GameManager.Instance.InputActions.Player.Move.ReadValue<Vector2>();
            if (moveRawValue == null)
            {
                DebugLogger.Log("Move vector is null");
                return;
            }
            var moveVector = transform.right * moveRawValue.x + transform.forward * moveRawValue.y;
            var moveDirection = MoveSpeed * Time.deltaTime * moveVector;
            controller.Move(moveDirection);
        }
    }
}
