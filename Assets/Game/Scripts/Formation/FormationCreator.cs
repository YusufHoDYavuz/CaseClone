using UnityEditor;
using UnityEngine;

namespace XGames.GameName
{
    [CustomEditor(typeof(PrefabSpawnerManager))]
    public class FormationCreator : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PrefabSpawnerManager spawner = (PrefabSpawnerManager)target;

            if (GUILayout.Button("Spawn Prefabs"))
            {
                spawner.SpawnPrefabs();
            }
        }
    }
}
