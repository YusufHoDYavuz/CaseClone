using UnityEngine;

namespace XGames.GameName
{
    public class Humanoid : MonoBehaviour
    {
        protected float health;
        protected bool isDeath;

        public virtual void TakeDamage(float damageAmount)
        {
            health -= damageAmount;

            if (health <= 0)
            {
                Die();
                health = 0;
            }
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
