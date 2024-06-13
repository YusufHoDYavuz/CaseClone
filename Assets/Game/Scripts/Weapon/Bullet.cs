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
            if (other.CompareTag("Enemy"))
            {
                gameObject.SetActive(false);

                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                    enemy.TakeDamage(damageAmount);
            }
        }
    }
}
