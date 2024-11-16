using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public List<GameObject> resourcePrefabs;       // List of resource prefabs to spawn
    public Vector2 spawnAreaMin;                   // Bottom-left corner of the spawn area
    public Vector2 spawnAreaMax;                   // Top-right corner of the spawn area
    public float minDistanceBetweenResources = 1.5f; // Minimum distance between resources
    public int chunkSizeMin = 3;                  // Minimum resources per chunk
    public int chunkSizeMax = 7;                  // Maximum resources per chunk
    public float chunkRadius = 3f;                // Radius of each chunk

    void Start()
    {
        SpawnResources();
    }

    void SpawnResources()
    {
        foreach (GameObject resourcePrefab in resourcePrefabs)
        {
            // Determine the number of chunks for this resource type
            int totalChunks = Random.Range(2, 5); // Number of chunks per resource type

            for (int chunk = 0; chunk < totalChunks; chunk++)
            {
                // Generate a random position for the chunk center
                Vector2 chunkCenter = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                // Generate a random chunk size for this resource
                int chunkSize = Random.Range(chunkSizeMin, chunkSizeMax);

                for (int i = 0; i < chunkSize; i++)
                {
                    // Random position within the chunk's radius
                    Vector2 spawnPosition = chunkCenter + Random.insideUnitCircle * chunkRadius;

                    // Ensure the position is within the spawn area
                    spawnPosition.x = Mathf.Clamp(spawnPosition.x, spawnAreaMin.x, spawnAreaMax.x);
                    spawnPosition.y = Mathf.Clamp(spawnPosition.y, spawnAreaMin.y, spawnAreaMax.y);

                    // Check for overlapping resources
                    if (!IsPositionValid(spawnPosition))
                        continue;

                    // Instantiate the resource
                    Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    bool IsPositionValid(Vector2 position)
    {
        // Check for overlaps with other colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, minDistanceBetweenResources);
        return colliders.Length == 0; // Valid if no colliders are overlapping
    }

    // Optional: Draw the spawn area and chunk radius in the Scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaMax - spawnAreaMin);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, chunkRadius); // Example chunk radius (adjust dynamically)
    }
}
