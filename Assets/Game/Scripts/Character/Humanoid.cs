using UnityEngine;

namespace XGames.GameName
{
    public class Humanoid : MonoBehaviour
    {
        protected float health = 100f;

        public virtual void TakeDamage(float damageAmount)
        {
            health -= damageAmount;

            if (health < 0)
                Die();
        }

        protected virtual void Die()
        {
            Debug.Log(gameObject.name + " has died.");
        }

        protected virtual void Attack()
        {

        }
    }
}
