using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public struct TerrainType
{
    // Read only since structs are immutable
    public readonly string name;
    public readonly float height;
    public readonly Color colour;

    // Constructor to initialize the fields
    public TerrainType(string name, float height, Color colour)
    {
        this.name = name;
        this.height = height;
        this.colour = colour;
    }
}

public struct WorldData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public WorldData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}

public class WorldGenerator : MonoBehaviour
{
    // Note: unity limits this to 255 and 241 = 240 + 1 and 240 is divisble by 1, 2, 4, 6, 8, 10, 12...
    public const int mapChunkSize = 241;
    public static int detail = 0;
    public static float noiseScale = 1000f;

    public static int octaves = 4;
    // Can change for detail
    public static float persistence = 0.5f;
    public static float lacunarity = 0.2f;

    public int seed = 0;

    public static float meshHeightMultiplier = 100;
    public AnimationCurve meshHeightCurve;

    public static WorldDisplay display;

    public static TerrainType[] regions = new TerrainType[]
    {
        new TerrainType("Water", 0.2f, Color.blue),
        new TerrainType("Forest", 0.5f, Color.green),
        new TerrainType("Mountain", 0.9f, Color.gray),
        new TerrainType("Snow", 1.0f, Color.white)
    };

    // Queue for the tasks (
    Queue<WorldThreadInfo<WorldData>> worldDataThreadQueue = new Queue<WorldThreadInfo<WorldData>>();

    // 
    struct WorldThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public WorldThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    public void RequestWorldData()
    {
        // Generate the world data

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

        Texture2D texture = TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize);
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, detail);

        WorldData worldData = DrawWorldData(colourMap, noiseMap, texture, meshData);
        MeshData meshInfo = MeshGenerator.GenerateTerrainMesh(worldData.heightMap, meshHeightMultiplier, meshHeightCurve, detail);
    }

    private void Start()
    {
        display = FindObjectOfType<WorldDisplay>();
    }

    public WorldData DrawWorldData(Color[] colourMap, float[,] noiseMap, Texture2D texture, MeshData meshData)
    {
        //WorldDisplay display = FindObjectOfType<WorldDisplay>();
        display.DrawTexture(texture);
        //display.DrawNoiseMap(noiseMap);
        display.DrawMesh(meshData, texture);

        return new WorldData(noiseMap, colourMap);
    }

    public void GenerateWorldData()
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
    }
}