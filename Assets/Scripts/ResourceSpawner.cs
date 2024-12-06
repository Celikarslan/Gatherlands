using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject stonePrefab;
    public GameObject coalPrefab;
    public GameObject ironPrefab;
    public GameObject goldPrefab;

    public TileBase grassTile; // Grass tile reference
    public int resourcesPerChunk = 10; // Number of resources to spawn per chunk
    public Tilemap groundTilemap; // Reference to the ground tilemap

    private HashSet<Vector2Int> spawnedPositions = new HashSet<Vector2Int>();

    public void SpawnResourcesWithTileCheck(Vector2Int chunkPosition, Tilemap tilemap)
    {
        int chunkSize = 16; // Assuming 16x16 chunks
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int i = 0; i < resourcesPerChunk; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(startX, startX + chunkSize),
                Random.Range(startY, startY + chunkSize)
            );

            if (spawnedPositions.Contains(randomPosition)) continue;

            TileBase tile = tilemap.GetTile(new Vector3Int(randomPosition.x, randomPosition.y, 0));
            if (tile == null) continue;

            if (tile == grassTile)
            {
                // Spawn trees only on grass
                if (Random.value < 0.5f) // 50% chance for tree
                {
                    SpawnResource(treePrefab, randomPosition);
                    spawnedPositions.Add(randomPosition);
                }
            }
            else
            {
                // Spawn ores or stones on non-grass tiles
                string resourceType = Random.value < 0.5f ? "Stone" : GetRandomOreType();
                GameObject prefab = GetPrefabForResource(resourceType);

                if (prefab != null)
                {
                    SpawnResource(prefab, randomPosition);
                    spawnedPositions.Add(randomPosition);
                }
            }
        }
    }

    private string GetRandomOreType()
    {
        float rand = Random.value;
        if (rand < 0.5f) return "Coal"; // 50% chance for coal
        if (rand < 0.8f) return "Iron"; // 30% chance for iron
        return "Gold"; // 20% chance for gold
    }

    private GameObject GetPrefabForResource(string resourceType)
    {
        switch (resourceType)
        {
            case "Tree":
                return treePrefab;
            case "Stone":
                return stonePrefab;
            case "Coal":
                return coalPrefab;
            case "Iron":
                return ironPrefab;
            case "Gold":
                return goldPrefab;
            default:
                return null;
        }
    }

    private void SpawnResource(GameObject prefab, Vector2Int position)
    {
        Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);
        Vector3 worldPosition = groundTilemap.CellToWorld(cellPosition);
        worldPosition += new Vector3(0.5f, 0.5f, 0); // Center the resource on the tile

        GameObject resource = Instantiate(prefab, worldPosition, Quaternion.identity);
        resource.transform.SetParent(transform); // Organize under ResourceSpawner
    }
}
