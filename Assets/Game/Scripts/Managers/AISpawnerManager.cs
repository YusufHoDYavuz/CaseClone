using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class AISpawnerManager : MonoBehaviour
    {
        [Header("Direction")]
        [SerializeField] private float radius;
        [SerializeField] private float angle;

        [Header("Spawner")]
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private int minEnemyCount;
        [SerializeField] private int maxEnemyCount;

        void Start()
        {
            SpawnEnemy();
            MainCharacterPlace();
        }

        private void SpawnEnemy()
        {
            int enemyCount = GetRandomEnemyCount();

            for (int i = 0; i < enemyCount; i++)
            {
                int randomValue = Random.Range(0, enemyPrefabs.Count);
                Instantiate(enemyPrefabs[randomValue], Vector3.zero, Quaternion.Euler(0,180,0), transform);
            }
        }

        public void MainCharacterPlace()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 childLocalPositions = GetMainCharacterLocalPositions(i);
                transform.GetChild(i).localPosition = childLocalPositions;
            }
        }

        private Vector3 GetMainCharacterLocalPositions(int index)
        {
            float x = radius * Mathf.Sqrt(index) * Mathf.Cos(Mathf.Deg2Rad * index * angle);
            float z = radius * Mathf.Sqrt(index) * Mathf.Sin(Mathf.Deg2Rad * index * angle);

            return new Vector3(x, 0, z);
        }
    
        private int GetRandomEnemyCount()
        {
            int randomValue = Random.Range(minEnemyCount, maxEnemyCount);

            return randomValue;
        }
    }
}
