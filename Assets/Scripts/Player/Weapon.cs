using System;
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
        public float BulletSpeed = 1f;
        [Tooltip("More = faster")]
        public float BulletFireRate = 5f;

        public bool IsFiring { get; private set; } = false;

        private List<Bullet> bulletPool;
        private int poolInitSize = 20;
        private float firingTimer;

        private void Awake()
        {
            bulletPool = new List<Bullet>();
            for (int i = 0; i < poolInitSize; i++) 
                AddNewBulletToPool();
        }
        
        private void Start()
        {
            // GameManager.Instance.InputActions.Player.Attack.performed += OnAttackPerformed;
            GameManager.Instance.InputActions.Player.Attack.started += OnAttackStarted;
            GameManager.Instance.InputActions.Player.Attack.canceled += OnAttackCanceled;
        }

        private void Update()
        {

            if (IsFiring)
            {
                if (BulletFireRate <= 0f)
                    DebugLogger.Log("BulletFireRate is too small");
                else
                {                    
                    firingTimer += Time.deltaTime;
                    if (firingTimer > 1f / BulletFireRate)
                    {
                        ShootBullet();
                        firingTimer = 0f;
                    }
                }
            }
        }

        public void ReturnToPool(Bullet bullet)
        {
            bullet.transform.parent = BulletPoolParent;
            bullet.transform.localPosition = Vector3.zero;
            bullet.gameObject.SetActive(false);
        }

        private void OnAttackStarted(InputAction.CallbackContext context)
        {
            IsFiring = true;
            ShootBullet();
        }

        private void OnAttackCanceled(InputAction.CallbackContext context)
        {
            IsFiring = false;
            firingTimer = 0f;
        }

        private void ShootBullet()
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
