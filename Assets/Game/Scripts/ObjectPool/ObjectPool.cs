using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;

        private List<GameObject> poolObjects = new List<GameObject>();

        [Header("Pool")]
        [Space(5)]
        [SerializeField] private GameObject bulletPrefab;
        [Range(1,100)] [SerializeField] private int amountToPool;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.SetActive(false);
                poolObjects.Add(obj);
            }
        }

        public GameObject GetPoolObjects()
        {
            for (int i = 0; i < poolObjects.Count; i++)
            {
                if (!poolObjects[i].activeInHierarchy)
                {
                    return poolObjects[i];
                }
            }

            return null;
        }
    }
}
