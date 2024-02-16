using UnityEngine;
using System.Collections; // Needed for IEnumerator

public class PlayerShooting : MonoBehaviour
{
    public Transform player;
    public Transform reticle;
    public GameObject singleShotBulletPrefab; // Prefab for single shot
    public GameObject shotgunBulletPrefab; // Prefab for shotgun spread
    public GameObject railgunBulletPrefab; // Prefab for railgun "particle"

    private enum FiringMode { Single, Shotgun, Railgun }
    private FiringMode currentMode = FiringMode.Single;
    private bool canShoot = true; // Flag to control shooting
    private float singleShotCooldown = 0.5f; // Cooldown time for single shot
    private float shotgunCooldown = 1f; // Cooldown time for shotgun
    private float railgunChargeTime = 0.66f; // Charge time for railgun before firing
    private float railgunFireRate = 0.02f; // Time between each "particle" prefab instantiation
    private int railgunBurstCount = 10; // Number of "particles" fired in one burst
    private Coroutine shootingCoroutine; // Reference to the shooting coroutine

    void Update()
    {
        HandleModeSwitch();

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            shootingCoroutine = StartCoroutine(Shoot());
        }
    }

    void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentMode != FiringMode.Single)
        {
            currentMode = FiringMode.Single;
            ResetShooting();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentMode != FiringMode.Shotgun)
        {
            currentMode = FiringMode.Shotgun;
            ResetShooting();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentMode != FiringMode.Railgun)
        {
            currentMode = FiringMode.Railgun;
            ResetShooting();
        }
    }

    void ResetShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine); // Stop the current shooting coroutine
        }
        canShoot = true; // Immediately allow shooting in the new mode
    }

    IEnumerator Shoot()
    {
        canShoot = false; // Disable shooting

        Vector2 direction = (reticle.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        switch (currentMode)
        {
            case FiringMode.Single:
                Instantiate(singleShotBulletPrefab, reticle.position, Quaternion.Euler(0f, 0f, angle));
                yield return new WaitForSeconds(singleShotCooldown);
                break;
            case FiringMode.Shotgun:
                ShootShotgun(angle);
                yield return new WaitForSeconds(shotgunCooldown);
                break;
            case FiringMode.Railgun:
                yield return new WaitForSeconds(railgunChargeTime); // Wait for the railgun to charge
                for (int i = 0; i < railgunBurstCount; i++)
                {
                    Instantiate(railgunBulletPrefab, reticle.position, Quaternion.Euler(0f, 0f, angle));
                    yield return new WaitForSeconds(railgunFireRate);
                }
                // Add a cooldown after the burst if needed
                break;
        }

        canShoot = true; // Re-enable shooting
    }

    void ShootShotgun(float baseAngle)
    {
        int pellets = 8;
        float spreadAngle = 45f;
        float angleStep = spreadAngle / (pellets - 1);
        float startingAngle = baseAngle - spreadAngle / 2;

        for (int i = 0; i < pellets; i++)
        {
            Instantiate(shotgunBulletPrefab, reticle.position, Quaternion.Euler(0f, 0f, startingAngle + angleStep * i));
        }
    }
}
