using UnityEngine;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    [RequireComponent(typeof(Animator))]
    public class Character : Humanoid
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1.0f;
        private float previousMousePositionX;
        private float mouseDeltaX = 0f;
        private bool isDragging;

        [Header("Animator")]
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
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

                Vector3 movement = new Vector3(mouseDeltaX * moveSpeed * Time.deltaTime, 0, 0);
                transform.Translate(movement);

                //Animation
                if (Mathf.Abs(mouseDeltaX) > 0.1f)
                {
                    animator.SetFloat("Horizontal", mouseDeltaX);
                }
            }
            else if(!isDragging && Mathf.Abs(mouseDeltaX) < 0.1f)
            {
                animator.SetFloat("Horizontal", 0);
            }
        }

        protected override void Attack()
        {

        }
    }
}
