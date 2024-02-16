using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public Transform player; // Player's transform
    public float distanceFromPlayer = 1.0f; // Fixed distance from the player to the reticle

    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        // Convert the mouse position to world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        // Ensure the reticle stays in the 2D plane
        mouseWorldPosition.z = player.position.z;

        // Calculate the direction from the player to the mouse cursor
        Vector3 direction = (mouseWorldPosition - player.position).normalized;

        // Set the reticle's position to be at the specified distance from the player, in the direction of the mouse cursor
        transform.position = player.position + direction * distanceFromPlayer;
    }
}
