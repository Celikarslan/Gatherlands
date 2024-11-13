using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;  // Drag the player object here in the Inspector
    public Vector3 offset;             // Offset to keep the camera a bit above the player
    public float smoothSpeed = 0.125f; // Controls how smoothly the camera follows

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
