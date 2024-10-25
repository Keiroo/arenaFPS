using UnityEngine;

namespace ArenaFPS.Scripts
{
    public abstract class Enemy : MonoBehaviour
    {
        public EnemyType EnemyType;
        [Tooltip("Accepts numbers separated by comma, and/or numbers range, e.g. \"0, 1, 3-7\"")]
        public string SpawnLevels = "";
        public int Health = 1;
        public float SpawnInterval = 1f;

        public int[] GetSpawnLevels()
        {
            return SpawnLevels.ConvertToIntArray();
        }

        public virtual void Damage(int value)
        {
            Health -= value;
            if (value <= 0)
                Kill();            
        }

        public virtual void Kill()
        {

        }
    }
}
