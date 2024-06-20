using UnityEngine;

namespace XGames.GameName
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed;
        [SerializeField] private Vector3 offset;

        [Header("Limit")]
        [SerializeField] private float limit;

        private void LateUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, -limit, limit);
            transform.position = smoothedPosition;
        }
    }
}
