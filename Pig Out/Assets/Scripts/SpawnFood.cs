using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [Header("Food Settings")]
    public GameObject[] foodPrefabs;
    public int foodTargetCount = 50;
    public float spawnRadius = 3f;
    public float spawnHeight = 10f;

    [Header("Spawn Timing")]
    public float spawnInterval = 0.3f;     // Delay between spawns
    public int minFoodThreshold = 15;      // Trigger refill below this

    private float spawnTimer = 0f;
    private List<GameObject> trackedFoods = new List<GameObject>();
    private bool isSpawning = true;

    void Update()
    {
        // Clean up destroyed or null entries
        trackedFoods.RemoveAll(f => f == null);

        // Start spawning if food count is low
        if (!isSpawning && trackedFoods.Count < minFoodThreshold)
        {
            isSpawning = true;
        }

        // Spawn food with interval while below target
        if (isSpawning && trackedFoods.Count < foodTargetCount)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnFoodItem();
                spawnTimer = 0f;
            }
        }
        else
        {
            isSpawning = false;
        }
    }

    void SpawnFoodItem()
    {
        if (foodPrefabs.Length == 0) return;

        // Pick random food
        GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Length)];

        // Spawn around this spawner
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            spawnHeight,
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Instantiate and track
        GameObject newFood = Instantiate(prefab, spawnPos, Quaternion.identity);
        trackedFoods.Add(newFood);
    }
}