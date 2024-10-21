using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("General Settings")]
        public float MoveSpeed = 10f;
        public float MoveSmoothTime = 1f;
        public float GravityStrength = 1f;
        public float JumpStrength = 1f;
        public float DashSmoothTime = 1f;
        public float DashStrength = 1f;

        [Header("Ground Settings")]
        public Transform GroundCheckTransform;
        public LayerMask GroundMask;
        public float GroundDistance = 0.2f;

        public bool IsMoving { get; private set; } = false;
        public bool IsGrounded { get; private set; } = true;
        public bool IsDashing { get; private set; } = false;
        
        private CharacterController controller;
        private Tween dashTween = null;
        private Vector3 currentMoveVelocity;
        private Vector3 moveDampVelocity;
        private Vector3 currentForceVelocity;
        private Vector3 currentDashVelocity;
        private float gravity;
        private bool jumpPerformed = false;
        private bool dashPerformed = false;

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
            GameManager.Instance.InputActions.Player.Dash.performed += OnDashPerformed;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
        }

        private void CalculateMovement()
        {
            var moveRawValue = GameManager.Instance.InputActions.Player.Move.ReadValue<Vector2>();
            if (moveRawValue == null)
            {
                DebugLogger.Log("Move vector is null");
                return;
            }

            currentForceVelocity = new Vector3(0f, currentForceVelocity.y, 0f);
            Move(moveRawValue);
            JumpAndGravity();
            Dash(moveRawValue);
            
            controller.Move(currentForceVelocity * Time.deltaTime);
        }

        private void Move(Vector2 moveRawValue)
        {
            var moveVector = transform.TransformDirection(new Vector3(moveRawValue.x, 0f, moveRawValue.y));
            currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, moveVector * MoveSpeed, ref moveDampVelocity, MoveSmoothTime);
            controller.Move(currentMoveVelocity * Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, GroundDistance, GroundMask);
            if (IsGrounded && currentForceVelocity.y < 0f)
                currentForceVelocity.y = -2f;

            if (jumpPerformed)
            {
                jumpPerformed = false;
                if (IsGrounded)
                    currentForceVelocity.y = Mathf.Sqrt(JumpStrength * -2f * gravity);
            }
            currentForceVelocity.y += gravity * GravityStrength * Time.deltaTime;
        }

        private void Dash(Vector2 moveRawValue)
        {
            if (dashPerformed)
            {
                dashPerformed = false;
                if (IsDashing || moveRawValue == Vector2.zero)
                    return;

                IsDashing = true;
                currentDashVelocity = transform.TransformDirection(new Vector3(moveRawValue.x, 0f, moveRawValue.y) * DashStrength);
                dashTween = DOTween.To(
                    () => currentDashVelocity, x => currentDashVelocity = x, Vector3.zero, DashSmoothTime)
                    .SetEase(Ease.OutQuart)
                    .OnComplete(() => {
                        currentDashVelocity = Vector3.zero;
                        IsDashing = false;
                        dashTween = null;
                    });
            }

            if (IsDashing)
            {
                var scaledVelocity = new Vector3(100f * Time.deltaTime * currentDashVelocity.x, currentForceVelocity.y, 100f * Time.deltaTime * currentDashVelocity.z);
                currentForceVelocity = scaledVelocity;
            }
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

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            dashPerformed = true;
        }
    }
}
