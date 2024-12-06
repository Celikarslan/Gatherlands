using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase dirtTile, sandTile, grassTile, snowTile;

    public int chunkSize = 16; // Number of tiles per chunk
    public float baseNoiseScale = 0.1f; // Adjust for smooth terrain
    public int octaves = 3; // Number of noise layers to combine
    public float persistence = 0.5f; // Controls amplitude of each octave
    public float lacunarity = 2.0f; // Controls frequency of each octave

    public int horizontalRenderRange = 3; // Number of chunks to generate horizontally
    public int verticalRenderRange = 3; // Number of chunks to generate vertically

    private Transform player;
    private Vector2Int lastPlayerChunk;
    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();
    private float randomSeedX;
    private float randomSeedY;

    public ResourceSpawner resourceSpawner; // Reference to ResourceSpawner


    void Start()
    {
        // Find the player and determine their initial chunk position
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerChunk = GetChunkPosition(player.position);

        // Set random seeds for noise generation
        randomSeedX = Random.Range(-10000f, 10000f);
        randomSeedY = Random.Range(-10000f, 10000f);

        // Generate the initial terrain around the player
        GenerateChunksAroundPlayer();
    }

    void Update()
    {
        Vector2Int currentChunk = GetChunkPosition(player.position);

        // If the player has moved to a new chunk, generate new terrain
        if (currentChunk != lastPlayerChunk)
        {
            lastPlayerChunk = currentChunk;
            GenerateChunksAroundPlayer();
        }
    }

    Vector2Int GetChunkPosition(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkY = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(chunkX, chunkY);
    }

    void GenerateChunksAroundPlayer()
    {
        Vector2Int currentChunk = GetChunkPosition(player.position);

        // Generate a grid of chunks around the player
        for (int xOffset = -horizontalRenderRange; xOffset <= horizontalRenderRange; xOffset++)
        {
            for (int yOffset = -verticalRenderRange; yOffset <= verticalRenderRange; yOffset++)
            {
                Vector2Int chunkPosition = new Vector2Int(
                    currentChunk.x + xOffset,
                    currentChunk.y + yOffset
                );

                // Generate the chunk if it hasn't been created yet
                if (!generatedChunks.Contains(chunkPosition))
                {
                    GenerateChunk(chunkPosition);
                    generatedChunks.Add(chunkPosition);
                }
            }
        }
    }

    void GenerateChunk(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int x = 0; x < chunkSize; x++)
        {
            int worldX = startX + x;

            for (int y = 0; y < chunkSize; y++)
            {
                int worldY = startY + y;

                // Generate noise-based terrain using octaves
                float perlinValue = GeneratePerlinNoise(worldX, worldY);

                // Assign tiles based on the Perlin noise value
                TileBase tileToPlace;
                if (perlinValue < 0.3f)
                    tileToPlace = sandTile; // Sand biome
                else if (perlinValue < 0.7f)
                    tileToPlace = grassTile; // Grass biome
                else if (perlinValue < 0.8f)
                    tileToPlace = dirtTile; // Dirt biome
                else
                    tileToPlace = snowTile; // Snow biome

                tilemap.SetTile(new Vector3Int(worldX, worldY, 0), tileToPlace);
            }
        }

        // Spawn resources in this chunk
        resourceSpawner.SpawnResourcesWithTileCheck(chunkPosition, tilemap);
    }

    float GeneratePerlinNoise(int worldX, int worldY)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0f;

        // Combine multiple octaves of Perlin noise
        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (worldX + randomSeedX) * baseNoiseScale * frequency;
            float sampleY = (worldY + randomSeedY) * baseNoiseScale * frequency;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // Scale to -1 to 1
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        // Normalize noise height to 0-1
        return Mathf.InverseLerp(-1f, 1f, noiseHeight);
    }
}
