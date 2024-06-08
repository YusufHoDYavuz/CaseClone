using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Weapon", fileName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        public int id;
        public float damage;
        public float fireRate;
    }
}
