using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class Enemy : Humanoid
    {
        [Header("Nav Mesh")]
        [SerializeField] private Animator animator;
        [SerializeField] private float attackRepeatTime;
        [SerializeField] private float maxAttackDistance;
        [SerializeField] private float damageAmount;
        private NavMeshAgent agent;
        private bool playerInAttackRange;
        private bool alreadyAttacked;

        private RaycastHit hit;
        private bool isHit;

        private GameObject target;

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
            target = e.character;
        }

        private void Update()
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < agent.stoppingDistance)
            {
                if (!target.GetComponent<Character>().GetIsDeath())
                    Attack();
                else
                    animator.SetTrigger("Idle");
            }
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
            if (!alreadyAttacked)
            {
                agent.SetDestination(transform.position);
                transform.LookAt(target.transform);

                animator.SetTrigger("Attack");
                animator.SetBool("isChase", false);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), attackRepeatTime);

                isHit = Physics.Raycast(transform.position, transform.forward, out hit, maxAttackDistance);
                if (isHit)
                {
                    Character player = hit.collider.GetComponent<Character>();
                    if (player != null)
                    {
                        StartCoroutine(AttackDamageDelay(player, 1f));
                    }
                }

                Debug.Log($"Enemy is attacked.");
            }
        }

        private IEnumerator AttackDamageDelay(Character character, float delay)
        {
            yield return new WaitForSeconds(delay);
            character.TakeDamage(damageAmount);
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(target.transform.position);
            animator.SetBool("isChase", true);
        }

        private void OnDrawGizmos()
        {
            if (isHit)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Vector3 lineTracePosition = new Vector3(transform.position.x, transform.position.y * 3, transform.position.z);
            Gizmos.DrawLine(lineTracePosition, hit.point);
        }
    }
}
