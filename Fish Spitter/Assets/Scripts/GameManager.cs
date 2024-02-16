using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Assign this array in the Inspector with your 3 enemy prefabs
    public Transform playerTransform; // Assign the player's Transform in the Inspector
    public float spawnInterval = 2.0f; // Time in seconds between spawns

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy prefabs array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }

        // Randomly select an enemy prefab
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[index];

        // Instantiate the selected enemy prefab at a random position
        GameObject enemy = Instantiate(selectedPrefab, new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);

        // Get the SimpleEnemyAI script component from the instantiated enemy
        SimpleEnemyAI enemyAI = enemy.GetComponent<SimpleEnemyAI>();
        if (enemyAI != null)
        {
            // Set the enemy's player Transform to the playerTransform
            enemyAI.player = playerTransform;
        }
        else
        {
            Debug.LogError("Spawned enemy does not have a SimpleEnemyAI component.");
        }
    }
}
