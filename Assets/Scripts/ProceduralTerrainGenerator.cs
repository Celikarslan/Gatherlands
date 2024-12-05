using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase dirtTile, sandTile, grassTile, snowTile;
    public int width = 100;
    public int height = 20;
    public float noiseScale = 0.1f;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            // Generate terrain height using Perlin noise
            int terrainHeight = Mathf.FloorToInt(Mathf.PerlinNoise(x * noiseScale, 0) * height);

            for (int y = 0; y <= terrainHeight; y++)
            {
                TileBase tileToPlace;

                // Assign tiles based on height
                if (terrainHeight < height * 0.3f)
                    tileToPlace = sandTile; // Sand biome
                else if (terrainHeight < height * 0.6f)
                    tileToPlace = grassTile; // Grass biome
                else if (terrainHeight < height * 0.8f)
                    tileToPlace = dirtTile; // Dirt biome
                else
                    tileToPlace = snowTile; // Snow biome

                tilemap.SetTile(new Vector3Int(x, y, 0), tileToPlace);
            }
        }
    }
}
