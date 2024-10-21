using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float MoveSpeed = 10f;
        public float MoveSmoothTime = 1f;
        public float GravityStrength = 1f;
        public float JumpStrength = 1f;

        public Transform GroundCheckTransform;
        public LayerMask GroundMask;
        public float GroundDistance = 0.2f;

        public bool IsMoving { get; private set; } = false;
        
        private CharacterController controller;
        private Vector3 currentMoveVelocity;
        private Vector3 moveDampVelocity;
        private Vector3 currentForceVelocity;
        private float gravity;
        private bool jumpPerformed = false;
        private bool isGrounded = true;



        private void Awake()
        {
            if (!TryGetComponent(out controller))
                DebugLogger.Log("Controller not found");
            gravity = Physics.gravity.y;
        }

        private void Start()
        {
            GameManager.Instance.InputActions.Player.Move.started += OnMoveStart;
            GameManager.Instance.InputActions.Player.Move.canceled += OnMoveCancel;
            GameManager.Instance.InputActions.Player.Jump.performed += OnJumpPerformed;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            var moveRawValue = GameManager.Instance.InputActions.Player.Move.ReadValue<Vector2>();
            if (moveRawValue == null)
            {
                DebugLogger.Log("Move vector is null");
                return;
            }

            // Base movement
            var moveVector = transform.TransformDirection(new Vector3(moveRawValue.x, 0f, moveRawValue.y));
            currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, moveVector * MoveSpeed, ref moveDampVelocity, MoveSmoothTime);
            controller.Move(currentMoveVelocity * Time.deltaTime);

            // Jump and gravity movement
            isGrounded = Physics.CheckSphere(GroundCheckTransform.position, GroundDistance, GroundMask);
            if (isGrounded && currentForceVelocity.y < 0f)
                currentForceVelocity.y = -2f;

            if (jumpPerformed)
            {
                jumpPerformed = false;
                if (isGrounded)
                    currentForceVelocity.y = Mathf.Sqrt(JumpStrength * -2f * gravity);
            }
            currentForceVelocity.y += gravity * GravityStrength * Time.deltaTime;
            controller.Move(currentForceVelocity * Time.deltaTime);
        }

        private void OnMoveStart(InputAction.CallbackContext context)
        {
            IsMoving = true;
        }

        private void OnMoveCancel(InputAction.CallbackContext context)
        {
            IsMoving = false;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            jumpPerformed = true;
        }
    }
}
