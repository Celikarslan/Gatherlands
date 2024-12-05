using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // Drag your Player GameObject here in the Inspector
    public float smoothSpeed = 0.125f; // Adjust this for how smooth you want the camera movement to be
    public Vector3 offset; // Adjust this for the camera's position relative to the player

    void LateUpdate()
    {
        if (player != null)
        {
            // Get the player's position
            Vector3 desiredPosition = player.transform.position + offset;
            // Smoothly interpolate the camera's position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
