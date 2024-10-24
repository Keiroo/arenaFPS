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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CollisionTags != null)
            {
                if (CollisionTags.Any(x => other.gameObject.CompareTag(x)))
                {
                    ReturnToPool();
                }
            }
        }

        public void Shoot()
        {
            var direction = Weapon.transform.forward;
            var speed = Weapon.BulletSpeed;
            rBody.velocity = direction * speed;
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
    }
}
