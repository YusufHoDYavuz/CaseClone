using UnityEditor;
using UnityEngine;

namespace XGames.GameName
{
    public class PrefabSpawnerManager : MonoBehaviour
    {
        [Header("Prefab Settings")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private int prefabCount = 1;

        public void SpawnPrefabs()
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is not assigned.");
                return;
            }

            for (int i = 0; i < prefabCount; i++)
            {
                GameObject newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
                newPrefab.name = prefab.name + "_" + i;
            }
        }
    }
}
