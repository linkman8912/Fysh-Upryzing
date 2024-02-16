using UnityEngine;
using TMPro; // Updated namespace for TextMeshPro

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed, adjustable from the Inspector
    public float health = 100f; // Player health
    public TextMeshProUGUI gameOverText;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameOverText.gameObject.SetActive(false); // Ensure the game over text is hidden at start
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize(); // Normalize to prevent faster diagonal movement
    }

    void FixedUpdate()
    {
        Vector2 moveVelocity = movement * moveSpeed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // Call this method to apply damage to the player
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took damage. Current health: " + health);

        if (health <= 0)
        {
            Debug.Log("Player died!");
            gameOverText.gameObject.SetActive(true); // Show the game over text
            // Handle player death here (disable movement, show game over screen, etc.)
        }
    }

    // Detect collisions with enemies
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Make sure to assign the "Enemy" tag to your enemy objects
        {
            // Assume the enemy deals a fixed amount of damage for simplicity, you can adjust as needed
            float damageAmount = 10f;
            TakeDamage(damageAmount);
        }
    }
}
