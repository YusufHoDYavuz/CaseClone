using DG.Tweening;
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
            Debug.Log($"{gameObject.name} has died.");
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

        protected virtual void DamageEffect(SkinnedMeshRenderer mesh,Color targetColor, float damageEffectIntensity)
        {
            Color initialColor = mesh.material.color;

            Sequence colorTween = DOTween.Sequence();
            colorTween.Append(mesh.material.DOColor(targetColor * damageEffectIntensity, 0.05f));
            colorTween.Append(mesh.material.DOColor(initialColor, 0.05f));
        }
    }
}
