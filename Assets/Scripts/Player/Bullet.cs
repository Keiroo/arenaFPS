using System.Collections;
using System.Linq;
using UnityEngine;

namespace ArenaFPS.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [TagSelector]
        public string[] CollisionTags;
        public Weapon Weapon {get; set;}

        private Rigidbody rBody;

        private void Awake()
        {
            if (!TryGetComponent(out rBody))
                DebugLogger.Log("Rigidbody not found");

            var bulletCollision = GetComponentInChildren<BulletCollision>(true);
            if (bulletCollision == null)
                DebugLogger.Log("BulletCollision not found");
            else
                bulletCollision.OnCollision += OnCollision;        
        }

        public void Shoot()
        {
            // StartCoroutine(MockShootRoutine());
            var direction = Weapon.transform.forward;
            var speed = Weapon.BulletSpeed;
            rBody.velocity = direction * speed;
            // StartCoroutine(MockShootRoutine());
        }

        private void OnCollision(Collision other)
        {
            if (CollisionTags != null)
            {
                if (CollisionTags.Any(x => other.gameObject.CompareTag(x)))
                {
                    ReturnToPool();
                }
            }
        }

        private void ReturnToPool()
        {
            rBody.velocity = Vector3.zero;      
            Weapon.ReturnToPool(this);
        }

        private IEnumerator ShootCoroutine()
        {
            yield return null;
        }

        // private IEnumerator MockShootRoutine()
        // {
        //     yield return new WaitForSeconds(Random.Range(2f, 4f));
        //     ReturnToPool();
        // }
    }
}
