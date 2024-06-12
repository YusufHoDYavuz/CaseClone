using System.Collections.Generic;
using UnityEngine;
using XGames.GameName.EventSystem;

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

        private List<GameObject> formationCharacters = new();

        private void Start()
        {
            SetCharacterCount();
        }

        private void Update()
        {
            Movement();

            if (Input.GetKeyDown(KeyCode.Y))
            {
                int randomValue = Random.Range(0, 5);
                EventBus<UpdateWeapon>.Emit(this, new UpdateWeapon(randomValue));
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                RaiseFormationCount();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                DecreaseFormationCount();
            }
        }

        private void Movement()
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

        private void SetCharacterCount()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                formationCharacters.Add(transform.GetChild(i).gameObject);
                
                if (i > 0)
                    formationCharacters[i].SetActive(false);
            }
        }

        private void RaiseFormationCount()
        {
            foreach (GameObject formation in formationCharacters)
            {
                if (!formation.activeInHierarchy)
                {
                    formation.SetActive(true);
                    break;
                }
            }
        }

        private void DecreaseFormationCount()
        {
            int activeCharacterAmount = GetActiveChracterAmount();

            for (int i = formationCharacters.Count - 1; i > 0; i--)
            { 
                if (formationCharacters[i].activeInHierarchy && activeCharacterAmount > 1)
                {
                    formationCharacters[i].SetActive(false);
                    break;
                }
            }
        }

        private int GetActiveChracterAmount()
        {
            int amount = 0;

            foreach(GameObject chracter in formationCharacters)
            {
                if (chracter.activeInHierarchy)
                {
                    amount++;
                }
            }

            return amount;
        }

        protected override void Attack()
        {

        }
    }
}
