using UnityEngine;

namespace XGames.GameName
{
    public class Humanoid : MonoBehaviour
    {
        protected float health = 100f;
        protected bool isDeath;

        public virtual void TakeDamage(float damageAmount)
        {
            health -= damageAmount;

            if (health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            Debug.Log(gameObject.name + " has died.");
        }

        public virtual bool GetIsDeath()
        {
            if (health <= 0)
                return true;
            else
                return false;
        }

        protected virtual void Attack()
        {

        }
    }
}
