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

        private void Start()
        {

        }

        private void Update()
        {
            Movement();

            if (Input.GetKeyDown(KeyCode.Y))
            {
                int randomValue = Random.Range(0, 5);
                EventBus<UpdateWeapon>.Emit(this, new UpdateWeapon(randomValue));
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
                        animator.SetFloat("Horizontal", 0);
                    }
                }
            }
        }

        protected override void Attack()
        {

        }
    }
}
