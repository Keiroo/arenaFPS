using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArenaFPS.Scripts
{
    public class Weapon : MonoBehaviour
    {
        public Camera WeaponCamera;

        [Header("Bullet Settings")]
        public GameObject BulletPrefab;
        public Transform BulletPoolParent;
        public Transform BulletStartPoint;

        private List<Bullet> bulletPool;
        private int poolInitSize = 20;

        private void Awake()
        {
            bulletPool = new List<Bullet>();
            for (int i = 0; i < poolInitSize; i++) 
                AddNewBulletToPool();
        }
        
        private void Start()
        {
            GameManager.Instance.InputActions.Player.Attack.performed += OnAttackPerformed;
        }

        public void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.transform.parent = BulletPoolParent;
            bullet.transform.localPosition = Vector3.zero;
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            var bullet = bulletPool.Any(x => !x.gameObject.activeSelf) ? 
                bulletPool.FirstOrDefault(x => x.gameObject.activeSelf == false) : 
                AddNewBulletToPool();
            
            bullet.gameObject.SetActive(true);
            bullet.transform.parent = null;
            bullet.transform.position = BulletStartPoint.position;
            bullet.Shoot();
        }

        private Bullet AddNewBulletToPool()
        {
            var bulletGO = Instantiate(BulletPrefab, BulletPoolParent);
            bulletGO.SetActive(false);
            if (!bulletGO.TryGetComponent<Bullet>(out var bullet))
                DebugLogger.Log("Bullet script not found");
            else
            {
                bulletPool.Add(bullet);
                bullet.Weapon = this;
                return bullet;
            }
            return default;
        }

    }
}
