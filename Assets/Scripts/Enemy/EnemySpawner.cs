using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaFPS.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] EnemyPrefabs;

        private List<Enemy>[] enemiesPools;
        private Enemy[] enemies;
        private float[] counters;
        private int arraysLength;

        private void Awake()
        {
            arraysLength = EnemyPrefabs.Length;
            if (arraysLength <= 0)
                DebugLogger.Log("EnemyPrefabs array is empty");
            else
            {
                enemies = new Enemy[arraysLength];
                enemiesPools = new List<Enemy>[arraysLength];
                counters = new float[arraysLength];

                for (int i = 0; i < arraysLength; i++)
                {
                    if (!EnemyPrefabs[i].TryGetComponent<Enemy>(out var enemy))
                        DebugLogger.Log("GameObject is not enemy prefab");
                    else
                    {
                        enemies[i] = enemy;
                        var list = new List<Enemy>();
                        enemiesPools[i] = list;
                    }
                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < arraysLength; i++)
            {
                var canSpawn = enemies[i].GetSpawnLevels().Contains(GameManager.Instance.CurrentLevel);
                if (!canSpawn)
                    continue;
                counters[i] += Time.deltaTime;
                if (counters[i] > enemies[i].SpawnInterval)
                {
                    counters[i] = 0f;
                    Spawn(i);
                }
            }
        }

        private void Spawn(int index)
        {
            var enemy = enemiesPools[index].FirstOrDefault(x => !x.gameObject.activeSelf);
            GameObject enemyGO;
            if (enemy != default)
                enemyGO = enemy.gameObject;
            else
            {
                enemyGO = Instantiate(EnemyPrefabs[index]);
                if (!enemyGO.TryGetComponent(out enemy))
                    DebugLogger.Log("Spawned object does not have Enemy script");
                enemiesPools[index].Add(enemy);
            }
            enemyGO.transform.parent = null;
            enemyGO.transform.position = Vector3.zero;
        }
    }
}
