using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    #region Variables

    [SerializeField]
    List<GameObject> enemiesList;
    GameObject enemyPrefab; // The enemy to spawn
    public Transform player; // Player's transform
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;
    public float minSpawnInterval = 3000f; // milliseconds
    public float maxSpawnInterval = 10000f; // milliseconds

    float spawnChance = 50f; // Percentage chance of spawning an enemy

    [SerializeField]
    private GameObject currentEnemy;

    BaseEnemy enemy;

    #endregion

    #region Start Functions

    private void Awake() { }

    void Start()
    {
        player = PlayerCharacter.Instance.transform;

        SpawnEnemyAsync();
    }

    #endregion

    #region Base Functions

    async void SpawnEnemyAsync()
    {
        while (true)
        {
            if (enemy != null)
                if (enemy.enemyDead)
                {
                    Destroy(currentEnemy);
                    enemy = null;
                }

            // Wait for a random interval using Task.Delay
            int interval = Random.Range((int)minSpawnInterval, (int)maxSpawnInterval);

            await Task.Delay(interval);

            if (!ShouldSpawn())
            {
                Debug.Log("Spawn Chance SKIP!.");
            }
            // Check if there is no active enemy
            else if (currentEnemy == null)
            {
                // Try to find a random spawn position near the player on the NavMesh
                if (FindSpawnPosition(out Vector3 spawnPosition))
                {
                    enemyPrefab = enemiesList[Random.Range(0, enemiesList.Count)];

                    // Spawn the enemy and assign it to currentEnemy
                    currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                    enemy = currentEnemy.GetComponent<BaseEnemy>();

                    Debug.Log("SPAWNED ENEMY!");
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

    bool ShouldSpawn()
    {
        // Generate a random number and check if it's within the spawnChance
        float randomValue = Random.Range(0f, 100f);
        return randomValue <= spawnChance;
    }

    #endregion
}
