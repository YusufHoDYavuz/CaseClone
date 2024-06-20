using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using XGames.GameName.EventSystem;
using XGames.GameName.Managers;

namespace XGames.GameName
{
    public class Enemy : Humanoid
    {
        [Header("Attack")]
        [SerializeField] private float attackRepeatTime;
        [SerializeField] private float maxAttackDistance;
        [SerializeField] private float damageAmount;
        private NavMeshAgent agent;
        private bool playerInAttackRange;
        private bool alreadyAttacked;

        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("Health & UI")]
        [SerializeField] private float initialHealth;
        [SerializeField] private GameObject healthBarCanvas;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private Image healthBarImageDelay;
        [SerializeField] private Text healthBarText;

        [Header("Death")]
        [SerializeField] private GameObject deathParticle;

        [Header("Damage Effect")]
        [SerializeField] private SkinnedMeshRenderer damageEffectMesh;
        [SerializeField] private Color damageEffectColor;
        [SerializeField] private float damageEffectIntensity;

        //Raycast for enemy
        private RaycastHit hit;
        private bool isHit;

        //Other - Single
        private GameObject target;
        private Camera mainCamera;
        private CapsuleCollider coll;

        private void OnEnable()
        {
            EventBus<GetCharacterEvent>.AddListener(SetCharacterValue);
        }

        private void OnDisable()
        {
            EventBus<GetCharacterEvent>.RemoveListener(SetCharacterValue);
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            coll = GetComponent<CapsuleCollider>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            base.health = initialHealth;
            UpdateHealthBarUI();
            healthBarCanvas.SetActive(false);
        }

        private void SetCharacterValue(object sender, GetCharacterEvent e)
        {
            target = e.character;
        }

        private void Update()
        {
            //Chase - Attack

            if (target == null) return;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < agent.stoppingDistance)
            {
                if (!target.GetComponent<Character>().GetIsDeath())
                    Attack();
                else
                    animator.SetTrigger("Idle");
            }
            else
            {
                if (GameStateManager.Instance.GetGameState() == GameStateManager.GameState.Start && !GetIsDeath())
                {
                    ChasePlayer();
                }
                else if(GetIsDeath())
                {
                    agent.SetDestination(transform.position);
                }
            }

            //Health Bar UI Rotation
            Vector3 direction = mainCamera.transform.position - healthBarCanvas.transform.position;
            healthBarCanvas.transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
            base.DamageEffect(damageEffectMesh, damageEffectColor, damageEffectIntensity);

            UpdateHealthBarUI();
        }

        protected override void Die()
        {
            base.Die();

            animator.SetBool("isDie", true);
            healthBarCanvas.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            StartCoroutine(nameof(DieDelay));
            StopCoroutine(nameof(AttackDamageDelay));
        } 

        private IEnumerator DieDelay()
        {
            Destroy(coll);
            yield return new WaitForSeconds(2f);
            Vector3 particlePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z * 0.9f);
            Instantiate(deathParticle, particlePosition, Quaternion.identity);
            yield return new WaitForSeconds(.2f);
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

        private void UpdateHealthBarUI()
        {
            if (!healthBarCanvas.activeInHierarchy)
            {
                healthBarCanvas.SetActive(true);
                healthBarCanvas.transform.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.OutBack);
            }

            //Health Amount Text
            healthBarText.text = health.ToString();

            //Health Image
            float convertHealthValue = Mathf.InverseLerp(0, initialHealth, health);
            DOTween.To(() => healthBarImage.fillAmount, x => healthBarImage.fillAmount = x, convertHealthValue, 0.2f)
               .SetEase(Ease.OutQuad);

            DOTween.To(() => healthBarImageDelay.fillAmount, x => healthBarImageDelay.fillAmount = x, convertHealthValue, 1f)
               .SetEase(Ease.OutQuad);
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
