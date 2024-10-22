using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    public class WeaponShoot : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public Camera WeaponCamera;
        
        private void Start()
        {
            GameManager.Instance.InputActions.Player.Attack.performed += OnAttackPerformed;
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            var startPos = new Vector3(transform.position.x, WeaponCamera.transform.position.y, transform.position.z);
            var bullet = Instantiate(BulletPrefab, startPos, Quaternion.identity);
        }
    }
}
