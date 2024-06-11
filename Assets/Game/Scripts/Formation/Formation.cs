using UnityEngine;

namespace XGames.GameName
{
    public class Formation : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float angle;

        void Start()
        {

        }

        void Update()
        {
            MainCharacterPlace();
        }

        private void MainCharacterPlace()
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
    }
}