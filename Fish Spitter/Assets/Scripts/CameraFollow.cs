using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Player's transform to follow
    public Vector2 offset; // Offset from the player position
    public float smoothTime = 0.3f; // Smoothing time
    public float zoomLevel = 5f; // Adjust this value to zoom in or out

    private float velocityX = 0.0f; // Current velocity in X, this value is modified by SmoothDamp
    private float velocityY = 0.0f; // Current velocity in Y, this value is modified by SmoothDamp
    private Camera cam; // Reference to the Camera component

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component attached to this GameObject
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Determine the target position based on the offset from the player
            float targetPosX = player.position.x + offset.x;
            float targetPosY = player.position.y + offset.y;

            // Smoothly transition to the target position
            float posX = Mathf.SmoothDamp(transform.position.x, targetPosX, ref velocityX, smoothTime);
            float posY = Mathf.SmoothDamp(transform.position.y, targetPosY, ref velocityY, smoothTime);

            // Update the camera's position
            transform.position = new Vector3(posX, posY, transform.position.z);

            // Set the camera's orthographic size to zoom in or out
            cam.orthographicSize = zoomLevel;
        }
    }
}
