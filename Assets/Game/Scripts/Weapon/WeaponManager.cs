using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGames.GameName.Events.States;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapons Data")]
        [SerializeField] private List<WeaponData> weaponsData;

        [Header("Weapon Mesh")]
        [SerializeField] private List<GameObject> weaponsMesh;
        
        //Fire
        private float fireRate = 1.0f;
        private float damage = 1.0f;
        private Coroutine fireCoroutine;

        private Character character;

        private void OnEnable()
        {
            EventBus<UpdateWeaponEvent>.AddListener(UpdateWeapon);
            EventBus<GameStartEvent>.AddListener(SetAndStartFire);

            if (character != null && DataHolder.Instance != null)
            {
                ChangeWeaponAndUpdateData(DataHolder.Instance.ActiveWeaponID);
                //Debug.Log($"ActiveWeaponID: {DataHolder.Instance.ActiveWeaponID}");

                StartFire();
            }
        }

        private void OnDisable()
        {
            EventBus<UpdateWeaponEvent>.RemoveListener(UpdateWeapon);
            EventBus<GameStartEvent>.RemoveListener(SetAndStartFire);
        }

        private void Awake()
        {
            character = GetComponentInParent<Character>();
        }

        private void Start()
        {
            SetWeaponsMesh();
        }

        private void SetAndStartFire(object sender,GameStartEvent e)
        {
            if (character != null && !character.GetIsDeath())
                StartFire();
        }

        #region Weapon

        private void SetWeaponsMesh()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i >= 1)
                {
                    weaponsMesh[i].SetActive(false);
                }
                else
                {
                    weaponsMesh[i].SetActive(true);
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
            UpdateWeaponData();

            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            fireCoroutine = StartCoroutine(FireLoop());
        }

        private IEnumerator FireLoop()
        {
            while (character != null && !character.GetIsDeath())
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

        private void UpdateWeapon(object sender, UpdateWeaponEvent e)
        {
            ChangeWeaponAndUpdateData(e.weaponId);
            UpdateWeaponData();
            StartFire();
        }

        private void ChangeWeaponAndUpdateData(int nextWeapon)
        {
            int activeWeaponId = GetActiveWeaponId();
            DataHolder.Instance.ActiveWeaponID = nextWeapon;

            weaponsMesh[activeWeaponId].SetActive(false);
            weaponsMesh[nextWeapon].SetActive(true);
        }

        private void UpdateWeaponData()
        {
            int activeWeaponId = GetActiveWeaponId();
            fireRate = weaponsData[activeWeaponId].fireRate;
            damage = weaponsData[activeWeaponId].damage;
        }
        #endregion
    }

}
