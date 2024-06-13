using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGames.GameName
{
    public class Enemy : Humanoid
    {
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
        }

        protected override void Die()
        {
            base.Die();

            Destroy(gameObject);
        }

        protected override void Attack()
        {

        }
    }
}
