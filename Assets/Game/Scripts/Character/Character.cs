using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGames.GameName.Events.States;
using XGames.GameName.EventSystem;
using XGames.GameName.Managers;

namespace XGames.GameName
{
    public class Character : Humanoid
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private float moveLimit = 10.0f;
        private float previousMousePositionX;
        private float mouseDeltaX = 0f;
        private bool isDragging;

        [Header("Formation")]
        [SerializeField] private float formationAnimationSpeed;
        [SerializeField] private ParticleSystem raiseFormationParticle;
        private List<GameObject> formationCharacters = new();

        [Header("Health")]
        [SerializeField] private float initialHealth;

        [Header("Damage Effect")]
        [SerializeField] private SkinnedMeshRenderer damageEffectMesh;
        [SerializeField] private Color damageEffectColor;
        [SerializeField] private float damageEffectIntensity;

        private void OnEnable()
        {
            EventBus<UpdateCharacterFormation>.AddListener(UpdateFormationCount);
        }

        private void OnDisable()
        {
            EventBus<UpdateCharacterFormation>.RemoveListener(UpdateFormationCount);
        }

        private void Start()
        {
            EventBus<GetCharacterEvent>.Emit(this, new GetCharacterEvent(this.gameObject));
            SetCharacterCount();

            base.health = initialHealth;
        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if (GameStateManager.Instance.GetGameState() == GameStateManager.GameState.Start)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                    previousMousePositionX = Input.mousePosition.x;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }

                if (isDragging)
                {
                    mouseDeltaX = Input.mousePosition.x - previousMousePositionX;
                    previousMousePositionX = Input.mousePosition.x;

                    Vector3 newPosition = transform.position + new Vector3(mouseDeltaX * moveSpeed * Time.deltaTime, 0, 0);
                    newPosition.x = Mathf.Clamp(newPosition.x, -moveLimit, moveLimit);

                    transform.position = newPosition;

                    // Animation
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (Mathf.Abs(mouseDeltaX) > 0.5f)
                        {
                            Animator animator = transform.GetChild(i).GetComponent<Animator>();

                            if (animator != null)
                                animator.SetFloat("Horizontal", mouseDeltaX);
                        }
                    }
                }
                else if (!isDragging)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (Mathf.Abs(mouseDeltaX) <= 0.5f)
                        {
                            Animator animator = transform.GetChild(i).GetComponent<Animator>();

                            if (animator != null)
                                animator.SetFloat("Horizontal", 0);
                        }
                    }
                }
            }
        }

        #region Character Formation
        private void SetCharacterCount()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                formationCharacters.Add(transform.GetChild(i).gameObject);

                if (i > 0)
                {
                    formationCharacters[i].SetActive(false);
                    formationCharacters[i].transform.localScale = Vector3.zero;
                }
            }
        }

        private void RaiseFormationCount(int raiseCount)
        {
            int activatedCount = 0;

            foreach (GameObject formationCharacter in formationCharacters)
            {
                if (!formationCharacter.activeInHierarchy)
                {
                    formationCharacter.SetActive(true);
                    formationCharacter.transform.DOScale(Vector3.one, formationAnimationSpeed).SetEase(Ease.OutBack);

                    Vector3 particlePosition = new Vector3(formationCharacter.transform.position.x, formationCharacter.transform.position.y * 2, formationCharacter.transform.position.z);
                    Instantiate(raiseFormationParticle, particlePosition, Quaternion.identity);

                    activatedCount++;

                    if (activatedCount >= raiseCount)
                    {
                        break;
                    }
                }
            }
        }

        private void DecreaseFormationCount(int decreaseCount)
        {
            int activatedCount = 0;
            int activeCharacterAmount = GetActiveChracterAmount();

            for (int i = formationCharacters.Count - 1; i > 0; i--)
            {
                if (formationCharacters[i].activeInHierarchy && activeCharacterAmount > 1)
                {
                    formationCharacters[i].transform.DOScale(Vector3.zero, formationAnimationSpeed).SetEase(Ease.InBack);
                    StartCoroutine(SetActiveWithDelay(formationCharacters[i], formationAnimationSpeed));

                    activatedCount++;

                    if (activatedCount >= decreaseCount)
                    {
                        break;
                    }
                }
            }
        }

        private void UpdateFormationCount(object sender, UpdateCharacterFormation e)
        {
            if (e.isIncrease)
                RaiseFormationCount(e.updateFormationCount);
            else
                DecreaseFormationCount(e.updateFormationCount);
        }

        private int GetActiveChracterAmount()
        {
            int amount = 0;

            foreach (GameObject chracter in formationCharacters)
            {
                if (chracter.activeInHierarchy)
                {
                    amount++;
                }
            }

            return amount;
        }
        #endregion

        private IEnumerator SetActiveWithDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);

        }

        protected override void Attack()
        {
            //
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
            base.DamageEffect(damageEffectMesh, damageEffectColor, damageEffectIntensity);

            Debug.Log($"Player is taken damage. Health: {health}");
        }

        protected override void Die()
        {
            base.Die();

            for (int i = 0; i < transform.childCount; i++)
            {
                Animator animator = transform.GetChild(i).GetComponent<Animator>();

                if (animator != null)
                {
                    animator.SetTrigger("Death");
                    isDeath = true;
                    EventBus<GameOverEvent>.Emit(this, new GameOverEvent());
                }
            }
            Debug.Log($"Player is dead.");
        }
    }
}
