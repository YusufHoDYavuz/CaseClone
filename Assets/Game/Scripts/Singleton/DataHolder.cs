using UnityEngine;

namespace XGames.GameName
{
    public class DataHolder : MonoBehaviour
    {
        public static DataHolder Instance { get; private set; }

        private int activeWeaponID;
        private int levelIndex;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public int ActiveWeaponID
        {
            get { return activeWeaponID; }
            set { activeWeaponID = value; }
        }

        public int LevelIndex
        {
            get { return levelIndex; }
            set 
            { 
                levelIndex = value;
                Debug.Log($"Current Level Index: {levelIndex}");
            }
        }
    }
}
