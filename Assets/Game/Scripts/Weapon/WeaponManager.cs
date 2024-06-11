using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapons Data")]
        [SerializeField] private List<WeaponData> weaponsData;

        [Header("Weapons Mesh")]
        [SerializeField] private List<GameObject> weaponsMesh;
        
        //Fire
        private float fireRate = 1.0f;
        private float damage = 1.0f;
        private Coroutine fireCoroutine;

        private void OnEnable()
        {
            EventBus<UpdateWeapon>.AddListener(UpdateWeapon);
        }

        private void OnDisable()
        {
            EventBus<UpdateWeapon>.RemoveListener(UpdateWeapon);
        }

        void Start()
        {
            SetWeaponsMesh();

            UpdateWeaponData();
            StartFire();
        }

        #region Weapon

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

        private int GetActiveWeaponId()
        {
            for (int i = 0; i < weaponsMesh.Count; i++)
            {
                if (weaponsMesh[i].activeInHierarchy)
                {
                    return i;
                }
            }

            return 0;
        }
        #endregion

        #region Fire

        private void StartFire()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = StartCoroutine(FireLoop());
        }

        private IEnumerator FireLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);
                Fire();
            }
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

        private void UpdateWeapon(object sender, UpdateWeapon e)
        {
            ChangeWeaponAndUpdateData(e.weaponId);
            StartFire();
        }

        private void UpdateWeaponData()
        {
            int activeWeaponId = GetActiveWeaponId();
            fireRate = weaponsData[activeWeaponId].fireRate;
            damage = weaponsData[activeWeaponId].damage;
        }

        private void ChangeWeaponAndUpdateData(int nextWeapon)
        {
            int activeWeaponId = GetActiveWeaponId();

            weaponsMesh[activeWeaponId].SetActive(false);
            weaponsMesh[nextWeapon].SetActive(true);

            UpdateWeaponData();
        }
        #endregion
    }
}
