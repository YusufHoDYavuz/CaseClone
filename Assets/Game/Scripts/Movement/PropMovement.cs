using UnityEngine;
using XGames.GameName.Managers;

namespace XGames.GameName
{
    [RequireComponent(typeof(Rigidbody))]
    public class PropMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (GameStateManager.Instance.GetGameState() == GameStateManager.GameState.Start)
                rigidbody.velocity = Vector3.back * moveSpeed;
        }
    }
}
