using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    [CreateAssetMenu( menuName = "Scriptable Objects/Level 1", fileName = "Level 1")]
    public class LevelData : ScriptableObject
    {
        [Header("Object")]
        public List<GameObject> prefabs;

        [Header("Settings")]
        public float objectDistance;
    }
}
