using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Prefab for the tree resource
    public GameObject stonePrefab; // Prefab for the stone resource

    public Tilemap groundTilemap; // Reference to the ground tilemap
    public TileBase grassTile; // Reference to the grass tile
    public TileBase sandTile; // Reference to the sand tile

    public int treesPerChunk = 5; // Number of trees to spawn per chunk
    public int stonesPerChunk = 7; // Number of stones to spawn per chunk
    public int chunkSize = 16; // Size of each chunk

    private HashSet<Vector2Int> spawnedResources = new HashSet<Vector2Int>();

    public void SpawnResources(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        // Spawn trees
        for (int i = 0; i < treesPerChunk; i++)
        {
            Vector2Int position = GetRandomPositionInChunk(startX, startY);
            if (!spawnedResources.Contains(position) && IsValidTreeTile(position))
            {
                SpawnResource(treePrefab, position);
                spawnedResources.Add(position);
            }
        }

        // Spawn stones
        for (int i = 0; i < stonesPerChunk; i++)
        {
            Vector2Int position = GetRandomPositionInChunk(startX, startY);
            if (!spawnedResources.Contains(position) && IsValidStoneTile(position))
            {
                SpawnResource(stonePrefab, position);
                spawnedResources.Add(position);
            }
        }
    }

    private Vector2Int GetRandomPositionInChunk(int startX, int startY)
    {
        int x = Random.Range(startX, startX + chunkSize);
        int y = Random.Range(startY, startY + chunkSize);
        return new Vector2Int(x, y);
    }

    private bool IsValidTreeTile(Vector2Int position)
    {
        // Trees can only spawn on grass tiles
        TileBase tile = groundTilemap.GetTile(new Vector3Int(position.x, position.y, 0));
        return tile == grassTile; // Only allow trees on grass
    }

    private bool IsValidStoneTile(Vector2Int position)
    {
        // Stones can spawn on any tile other than grass
        TileBase tile = groundTilemap.GetTile(new Vector3Int(position.x, position.y, 0));
        return tile != grassTile && tile != null; // Allow stones on tiles other than grass
    }

    private void SpawnResource(GameObject resourcePrefab, Vector2Int position)
    {
        // Convert tilemap coordinates to world coordinates
        Vector3Int cellPosition = new Vector3Int(position.x, position.y, 0);
        Vector3 worldPosition = groundTilemap.CellToWorld(cellPosition);

        // Adjust the resource's position to align it with the tile
        worldPosition += new Vector3(0.5f, 0.5f, 0); // Center the resource on the tile

        // Instantiate the resource prefab at the calculated position
        GameObject resource = Instantiate(resourcePrefab, worldPosition, Quaternion.identity);

        // Parent the resource under the ResourceSpawner for organization
        resource.transform.SetParent(transform);
    }
}
