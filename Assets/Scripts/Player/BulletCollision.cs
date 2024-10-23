using UnityEngine;
using UnityEngine.Events;

namespace ArenaFPS.Scripts
{
    public class BulletCollision : MonoBehaviour
    {
        public UnityAction<Collision> OnCollision {get; set;}
        private void OnCollisionEnter(Collision other)
        {
            OnCollision?.Invoke(other);
        }
    }
}
