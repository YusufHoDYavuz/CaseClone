using UnityEngine;

namespace XGames.GameName
{
    public class PrefabManager : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float prefabPosition;

        void Start()
        {
            SetPrefabPosition();
        }

        private void SetPrefabPosition()
        {
            float randomXValue = GetRandomValue(prefabPosition, -prefabPosition);
            prefab.transform.localPosition = new Vector3(randomXValue, 0, 0);
        }

        private float GetRandomValue(float value1, float value2)
        {
            int randomValue = Random.Range(0, 2);
            return randomValue == 0 ? value1 : value2;
        }
    }
}
