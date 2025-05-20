using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [Header("Food Settings")]
    public GameObject[] foodPrefabs;           // Assign different food prefabs here
    public int foodCount = 20;                 // How many to spawn
    public float spawnRadius = 3f;             // Spread around center
    public float spawnHeight = 10f;            // How high above the table to spawn

    [Header("Spawn Timing")]
    public bool spawnOverTime = false;         // Toggle timed spawning
    public float spawnInterval = 0.5f;         // Time between spawns if using timed mode

    private float spawnTimer = 0f;
    private int spawned = 0;

    void Start()
    {
        if (!spawnOverTime)
        {
            for (int i = 0; i < foodCount; i++)
            {
                SpawnFoodItems();
            }
        }
    }

    void Update()
    {
        if (spawnOverTime && spawned < foodCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnFoodItems();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnFoodItems()
    {
        if (foodPrefabs.Length == 0) return;

        // Pick a random prefab
        GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Length)];

        // Pick a random X/Z position around the spawner
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            spawnHeight,
            Random.Range(-spawnRadius, spawnRadius)
        );

        Instantiate(prefab, spawnPos, Quaternion.identity);
        spawned++;
    }
}