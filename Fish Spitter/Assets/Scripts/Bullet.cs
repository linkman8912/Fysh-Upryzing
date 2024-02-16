using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float range = 5f; // Range of the bullet
    public float damage = 10f; // Damage the bullet will deal

    private Vector2 startPosition;

    void Start()
    {
        rb.velocity = transform.right * speed;
        startPosition = rb.position;
        rb.isKinematic = true; // Ensure the Rigidbody2D doesn't use physics-based movement
    }

    void Update()
    {
        // Destroy the bullet if it exceeds its range
        if (Vector2.Distance(startPosition, rb.position) > range)
        {
            Destroy(gameObject);
        }
    }

    // Adjusted to use OnCollisionEnter2D
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the object collided with has the "Enemy" tag
        if (hitInfo.CompareTag("Enemy"))
        {
            Debug.Log("Bullet collided with: " + hitInfo.name);

            // Attempt to get the SimpleEnemyAI component from the hit object
            SimpleEnemyAI enemy = hitInfo.GetComponent<SimpleEnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Damage applied to: " + hitInfo.name);
            }

            // Destroy the bullet since it hit an enemy
            Destroy(gameObject);
        }
    }
}