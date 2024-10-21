using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        private Transform parent;
        private float xRotation = 0f;

        private bool firstLook = true;

        private void Start()
        {
            parent = transform.parent;
            GameManager.Instance.InputActions.Player.Look.performed += OnLook;
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            // Discard first look event
            // In editor, first look always rotates to ground
            if (firstLook)
            {
                firstLook = false;
                DebugLogger.Log("Discard first look event");
                return;
            }

            var lookRawValue = context.ReadValue<Vector2>();
            if (lookRawValue == null)
            {
                DebugLogger.Log("Look vector is null");
                return;
            }
            var lookSensitivity = GameManager.Instance.Options.LookSensitivity;
            var lookDirection = 100f * lookSensitivity * Time.deltaTime * lookRawValue;

            xRotation -= lookDirection.y;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            parent.Rotate(Vector3.up * lookDirection.x);
        }
    }
}
