using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player; // Player's transform to target
    public float speed = 5.0f; // Movement speed towards/away from the player
    public float stoppingDistance = 2.0f; // Radius of the arc for strafing
    public float retreatDistance = 1.0f; // Distance at which the enemy starts retreating
    public float strafeSpeed = 30.0f; // Speed at which the enemy strafes around the player
    public GameObject bulletPrefab; // Prefab of the bullet to shoot
    public float shootingInterval = 2.0f; // Time between shots
    private float timeSinceLastShot = 0.0f; // Time since the last shot was fired
    public float bulletSpeed = 20f; // Speed of the bullet
    public float health = 50f; // Enemy health
    private float currentAngleAroundPlayer = 0f; // Current angle of the enemy around the player for arc movement
    public float trackingDelay = 0.5f; // Delay in seconds for the enemy to adjust its angle towards the player
    private float trackingTimer = 0f; // Timer to track the delay
    public float angleRandomness = 10f; // Maximum random angle offset for strafing

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Move towards the player if beyond stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            // Smoothly update the angle towards the player
            SmoothUpdateAngleTowardsPlayer();
        }
        // Strafe in an arc around the player within stopping distance
        else if (distanceToPlayer <= stoppingDistance && distanceToPlayer > retreatDistance)
        {
            // Smoothly increment the angle for strafing
            currentAngleAroundPlayer += strafeSpeed * Time.deltaTime;
            // Calculate target position based on new angle
            Vector2 targetPosition = new Vector2(
                player.position.x + stoppingDistance * Mathf.Cos(currentAngleAroundPlayer * Mathf.Deg2Rad),
                player.position.y + stoppingDistance * Mathf.Sin(currentAngleAroundPlayer * Mathf.Deg2Rad)
            );
            // Smoothly move to the new position
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * strafeSpeed);
        }
        // Move away from the player if too close
        else if (distanceToPlayer <= retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    void SmoothUpdateAngleTowardsPlayer()
    {
        float targetAngle = Mathf.Atan2(transform.position.y - player.position.y, transform.position.x - player.position.x) * Mathf.Rad2Deg;
        // Smoothly transition between current and target angles
        currentAngleAroundPlayer = Mathf.LerpAngle(currentAngleAroundPlayer, targetAngle, Time.deltaTime * strafeSpeed);
    }


    void UpdateAngleTowardsPlayer()
    {
        currentAngleAroundPlayer = Mathf.Atan2(transform.position.y - player.position.y, transform.position.x - player.position.x) * Mathf.Rad2Deg + Random.Range(-angleRandomness, angleRandomness);
    }

    void HandleShooting()
    {
        if (timeSinceLastShot >= shootingInterval)
        {
            ShootAtPlayer();
            timeSinceLastShot = 0f;
        }
        else
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
            rb.isKinematic = true; // Use kinematic to directly control the bullet's velocity
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("");
        }
    }
}
