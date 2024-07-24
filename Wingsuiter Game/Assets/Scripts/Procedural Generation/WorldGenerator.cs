using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

    // Constructor to initialize the fields
    public TerrainType(string name, float height, Color colour)
    {
        this.name = name;
        this.height = height;
        this.colour = colour;
    }
}

public class WorldGenerator : MonoBehaviour
{
    // Note: unity limits this to 255 and 241 = 240 + 1 and 240 is divisble by 1, 2, 4, 6, 8, 10, 12...
    public const int mapChunkSize = 241;
    public static int detail = 0;
    public static float noiseScale = 100f;

    public static int octaves = 4;
    // Can change for detail
    public static float persistence = 0.5f;
    public static float lacunarity = 0.2f;

    public int seed = 0;

    public static float meshHeightMultiplier = 100;
    public AnimationCurve meshHeightCurve;

    public static TerrainType[] regions = new TerrainType[]
    {
        new TerrainType("Water", 0.2f, Color.blue),
        new TerrainType("Forest", 0.5f, Color.green),
        new TerrainType("Mountain", 0.9f, Color.gray),
        new TerrainType("Snow", 1.0f, Color.white)
    };

    public void GenerateWorld()
    {
        // Get the noise map from the noise class
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        // Iterate over the noise map
        for (int x = 0; x < mapChunkSize; x++)
        {
            for (int y = 0; y < mapChunkSize; y++)
            {
                float currentHeight = noiseMap[x, y];

                // Iterate through the regions
                foreach (TerrainType region in regions)
                {
                    // 
                    if (currentHeight <= region.height)
                    {
                        // Get the colour...
                        colourMap[y * mapChunkSize + x] = region.colour;
                        break;
                    }
                }
            }
        }

        WorldDisplay display = FindObjectOfType<WorldDisplay>();
        display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        //display.DrawNoiseMap(noiseMap);
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, detail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
    }
}