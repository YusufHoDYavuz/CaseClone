using UnityEngine;

namespace XGames.GameName
{
    public class Formation : MonoBehaviour
    {
        [Header("Formation")]
        [SerializeField] private float characterSpacing;

        void Update()
        {
            MainCharacterPlace();
        }

        private void MainCharacterPlace()
        {
            int gridSize = Mathf.CeilToInt(Mathf.Sqrt(transform.childCount));

            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 childLocalPositions = GetMainCharacterLocalPositions(i, gridSize);
                transform.GetChild(i).localPosition = childLocalPositions;
            }
        }

        private Vector3 GetMainCharacterLocalPositions(int index, int gridSize)
        {
            int column = index % gridSize;
            int row = index / gridSize;

            float x = column * characterSpacing;
            float z = row * -characterSpacing;

            return new Vector3(x, 0, z);
        }
    }
}