using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class Enemy : Humanoid
    {
        [Header("Nav Mesh")]
        [SerializeField] private Animator animator;
        [SerializeField] private float attackRepeatTime;
        private NavMeshAgent agent;
        private bool playerInAttackRange;
        private bool alreadyAttacked;

        private Transform target;

        private void OnEnable()
        {
            EventBus<GetCharacter>.AddListener(SetAgentValue);
        }

        private void OnDisable()
        {
            EventBus<GetCharacter>.RemoveListener(SetAgentValue);
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void SetAgentValue(object sender, GetCharacter e)
        {
            target = e.character.transform;
        }

        private void Update()
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget < agent.stoppingDistance)
                Attack();
            else
                ChasePlayer();
        }

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
            agent.SetDestination(transform.position);

            transform.LookAt(target);

            if (!alreadyAttacked)
            {
                animator.SetTrigger("Attack");
                animator.SetBool("isChase", false);

                Debug.Log("Enemy is attacked.");

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), attackRepeatTime);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(target.position);
            animator.SetBool("isChase", true);
        }
    }
}
