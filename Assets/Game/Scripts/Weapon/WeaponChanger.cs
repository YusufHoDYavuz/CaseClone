using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XGames.GameName.Events.States;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class WeaponChanger : MonoBehaviour, IDamageable
    {
        [Header("Weapon")]
        [SerializeField] private List<GameObject> weaponMesh;
        private int weaponID;

        [Header("Health")]
        [SerializeField] private float hitAmount;
        [SerializeField] private TextMeshPro hitText;

        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("Particle")]
        [SerializeField] private GameObject explosionParticle;

        private void OnEnable()
        {
            EventBus<GameStartEvent>.AddListener(SetStartActions);
        }

        private void OnDisable()
        {
            EventBus<GameStartEvent>.RemoveListener(SetStartActions);
        }

        void Start()
        {
            SetRandomWeapon();

            int weaponID = GetRandomWeaponID();
            this.weaponID = weaponID;
            //Debug.Log($"Weapon ID: {weaponID}");

            hitText.text = hitAmount.ToString();
        }

        private void SetRandomWeapon()
        {
            int randomValue = Random.Range(0, weaponMesh.Count);

            for (int i = 0; i < weaponMesh.Count; i++)
            {
                weaponMesh[randomValue].SetActive(true);
            }
        }

        private int GetRandomWeaponID()
        {
            int weaponID = 0;

            foreach (GameObject weapon in weaponMesh)
            {
                if (weapon.activeInHierarchy)
                {
                    weaponID = weaponMesh.IndexOf(weapon);
                }
            }

            return weaponID;
        }

        public void Damage(float damageAmount)
        {
            animator.SetTrigger("Hit");

            hitAmount -= 1;
            hitText.text = hitAmount.ToString();

            if (hitAmount <= 0)
            {
                Vector3 particlePosition = new Vector3(transform.position.x, transform.position.y * -3, transform.position.z);
                Instantiate(explosionParticle, particlePosition, Quaternion.identity);

                EventBus<UpdateWeaponEvent>.Emit(this, new UpdateWeaponEvent(weaponID));
                Destroy(gameObject);
            }
        }

        private void SetStartActions(object sender,GameStartEvent e)
        {
            animator.SetTrigger("Roll");
        }
    }
}
