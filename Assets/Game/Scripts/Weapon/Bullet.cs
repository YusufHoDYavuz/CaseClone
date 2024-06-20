using UnityEngine;

namespace XGames.GameName
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        [Header("Attack")]
        [SerializeField] private float damageAmount;
        [SerializeField] private GameObject hitParticle;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = Vector3.forward * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Target"))
            {
                SetActiveBullet();

                IDamageable damageable = other.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(damageAmount);
                }
            }
        }

        private void SetActiveBullet()
        {
            Vector3 partilcePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z * 0.9f);
            Instantiate(hitParticle, partilcePosition, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
