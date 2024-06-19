using UnityEngine;

namespace XGames.GameName
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Weapon", fileName = "Weapon")]
    public class WeaponData : ScriptableObject
    {
        public int id;
        public float damage;
        public float fireRate;
    }
}
