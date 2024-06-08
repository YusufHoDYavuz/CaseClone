using UnityEngine;

namespace XGames.GameName
{
    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1.0f;
        private float previousMousePositionX;
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
                float mouseDeltaX = Input.mousePosition.x - previousMousePositionX;
                previousMousePositionX = Input.mousePosition.x;

                Vector3 movement = new Vector3(mouseDeltaX * moveSpeed * Time.deltaTime, 0, 0);
                transform.Translate(movement);

                //Animation
                if (Mathf.Abs(mouseDeltaX) > 0.2f)
                {
                    animator.SetFloat("Horizontal", mouseDeltaX);
                }
            }
            else
            {
                animator.SetFloat("Horizontal", 0);
            }
        }
    }
}
