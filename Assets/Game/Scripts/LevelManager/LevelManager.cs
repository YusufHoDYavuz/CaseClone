using System.Collections.Generic;
using UnityEngine;
using XGames.GameName.Events.States;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<LevelData> levels;
        [SerializeField] private Transform levelParentObject;
        [SerializeField] private float startPosition;
        private List<GameObject> levelObjects = new();

        private void OnEnable()
        {
            EventBus<GamePrepareEvent>.AddListener(CreateLevelObjects);
        }

        private void OnDisable()
        {
            EventBus<GamePrepareEvent>.RemoveListener(CreateLevelObjects);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                EventBus<GameEndEvent>.Emit(this, new GameEndEvent());
            }
        }

        private void CreateLevelObjects(object sender, GamePrepareEvent e)
        {
            if (levelObjects.Count > 0)
                ClearPreviousLevelData();

            for (int j = 0; j < levels[e.levelIndex].prefabs.Count; j++)
            {
                GameObject levelObject = Instantiate(levels[e.levelIndex].prefabs[j], levels[e.levelIndex].prefabs[j].transform.position, Quaternion.identity, levelParentObject);
                levelObject.transform.localPosition = new Vector3(levelObject.transform.position.x, levelObject.transform.position.y, startPosition);
                startPosition += levels[e.levelIndex].objectDistance;

                levelObjects.Add(levelObject);
            }
        }

        private void ClearPreviousLevelData()
        {
            startPosition = 0;

            foreach (GameObject levelObject in levelObjects)
            {
                Destroy(levelObject);
            }
        }
    }
}
