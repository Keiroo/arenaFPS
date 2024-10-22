using System.Collections;
using UnityEngine;

namespace ArenaFPS.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public Weapon Weapon {get; set;}

        public void Shoot()
        {
            StartCoroutine(MockShootRoutine());
        }

        private void ReturnToPool()
        {
            Weapon.ReturnToPool(this);
        }

        private IEnumerator MockShootRoutine()
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            ReturnToPool();
        }
    }
}
