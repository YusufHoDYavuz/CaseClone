using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapons Data")]
        [SerializeField] private List<ScriptableObject> weaponsData;

        private List<GameObject> weaponsMesh = new();

        void Start()
        {
            SetWeaponsMesh();
        }

        void Update()
        {
        
        }

        private void SetWeaponsMesh()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                weaponsMesh.Add(transform.GetChild(i).gameObject);
            }
        }
    }
}
