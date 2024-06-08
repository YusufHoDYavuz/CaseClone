using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapons Data")]
        [SerializeField] private List<ScriptableObject> weaponsData;

        [Header("Weapons Mesh")]
        [SerializeField] private List<GameObject> weaponsMesh;
        
        void Start()
        {
            SetWeaponsMesh();

            InvokeRepeating("Fire",1f,2f);
        }

        private void SetWeaponsMesh()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                weaponsMesh.Add(transform.GetChild(i).gameObject);

                if (i >= 1)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        private GameObject GetActiveWeapon()
        {
            foreach (GameObject weapon in weaponsMesh)
            {
                if (weapon.activeInHierarchy)
                {
                    return weapon;
                }
            }

            return null;
        }

        private void Fire()
        {
            GameObject bullet = ObjectPool.instance.GetPoolObjects();
            Transform activeWeaponBulletPosition = GetActiveWeapon().GetComponent<Weapon>().bulletPosition;

            if (bullet != null)
            {
                bullet.transform.position = activeWeaponBulletPosition.position;
                bullet.SetActive(true);
            }
        }
    }
}
