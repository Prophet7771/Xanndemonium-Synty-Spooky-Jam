using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy to spawn
    public Transform player; // Player's transform
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;
    public float minSpawnInterval = 3000f; // milliseconds
    public float maxSpawnInterval = 10000f; // milliseconds

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemyAsync();
    }

    async void SpawnEnemyAsync()
    {
        while (true)
        {
            // Wait for a random interval using Task.Delay
            int interval = Random.Range((int)minSpawnInterval, (int)maxSpawnInterval);
            await Task.Delay(interval);

            // Check if there is no active enemy
            if (currentEnemy == null)
            {
                // Try to find a random spawn position near the player on the NavMesh
                if (FindSpawnPosition(out Vector3 spawnPosition))
                {
                    // Spawn the enemy and assign it to currentEnemy
                    currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    bool FindSpawnPosition(out Vector3 spawnPosition)
    {
        // Generate a random position within the specified distance from the player
        Vector3 randomDirection = Random.insideUnitSphere * maxSpawnDistance;
        randomDirection += player.position;

        // Ensure the random position is within the desired range
        if (
            NavMesh.SamplePosition(
                randomDirection,
                out NavMeshHit hit,
                maxSpawnDistance,
                NavMesh.AllAreas
            )
        )
        {
            // Check if it's within min distance
            if (Vector3.Distance(hit.position, player.position) >= minSpawnDistance)
            {
                spawnPosition = hit.position;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }
}
