using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class GateManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gateObjects;
        [SerializeField] private float gateXPosition;

        private void Start()
        {
            SetRandomPositions();
        }

        private void SetRandomPositions()
        {
            if (gateObjects.Count != 2)
            {
                Debug.LogError("Exactly 2 objects are required.");
                return;
            }

            bool isFirstObjectPositive = Random.Range(0, 2) == 0;

            if (isFirstObjectPositive)
            {
                gateObjects[0].transform.localPosition = new Vector3(gateXPosition, 0, 0);
                gateObjects[1].transform.localPosition = new Vector3(-gateXPosition, 0, 0);
            }
            else
            {
                gateObjects[0].transform.localPosition = new Vector3(-gateXPosition, 0, 0);
                gateObjects[1].transform.localPosition = new Vector3(gateXPosition, 0, 0);
            }
        }
    }
}
