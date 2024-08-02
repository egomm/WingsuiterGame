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
    public const int mapChunkSize = 241;
    public static int detail = 0;//8;
    public static float noiseScale = 100f;//1000f;

    public static int octaves = 4;
    public static float persistence = 0.5f;
    public static float lacunarity = 0.2f;

    public int seed = 0;

    public static float meshHeightMultiplier = 10;//100;
    public AnimationCurve meshHeightCurve;

    public static WorldDisplay display;

    public static TerrainType[] regions = new TerrainType[]
    {
        new TerrainType("Water", 0.2f, Color.blue),
        new TerrainType("Forest", 0.5f, Color.green),
        new TerrainType("Mountain", 0.9f, Color.gray),
        new TerrainType("Snow", 1.0f, Color.white)
    };

    public void RequestWorldData(System.Action<WorldData> callback)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity);
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                foreach (TerrainType region in regions)
                {
                    if (currentHeight <= region.height)
                    {
                        colourMap[y * mapChunkSize + x] = region.colour;
                        break;
                    }
                }
            }
        }

        WorldData worldData = new WorldData(noiseMap, colourMap);
        callback(worldData);
    }
}
