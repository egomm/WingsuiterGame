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
    public static int worldWidth = 100;
    public static int worldHeight = 100;
    public static float noiseScale = 10f;

    public static int octaves = 4;
    // Can change for detail
    public static float persistence = 0.5f;
    public static float lacunarity = 0.2f;

    public int seed = 0;

    public static float meshHeightMultiplier = 10;
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
        float[,] noiseMap = Noise.GenerateNoiseMap(worldWidth, worldHeight, seed, noiseScale, octaves, persistence, lacunarity);

        Color[] colourMap = new Color[worldWidth * worldHeight];
        // Iterate over the noise map
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                float currentHeight = noiseMap[x, y];

                // Iterate through the regions
                foreach (TerrainType region in regions)
                {
                    // 
                    if (currentHeight <= region.height)
                    {
                        // Get the colour...
                        colourMap[y * worldWidth + x] = region.colour;
                        break;
                    }
                }
            }
        }

        WorldDisplay display = FindObjectOfType<WorldDisplay>();
        display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, worldWidth, worldHeight));
        //display.DrawNoiseMap(noiseMap);
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, worldWidth, worldHeight));
    }
}