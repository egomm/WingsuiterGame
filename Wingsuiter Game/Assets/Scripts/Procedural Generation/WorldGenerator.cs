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
    public void RequestWorldData(Action<WorldData> callback)
    {
        //
        ThreadStart threadStart = delegate
        {
            WorldDataThread(callback);
        };

        //
        new Thread(threadStart).Start();
    }

    void WorldDataThread(Action<WorldData> callback)
    {
        WorldData worldData = GenerateWorldData();
        // Ensure that only one thread can 
        lock (worldDataThreadQueue)
        {
            worldDataThreadQueue.Enqueue(new WorldThreadInfo<WorldData>(callback, worldData));
        }
    }

    private void Update()
    {
        // Check if the world data thread queue is not empty
        if (worldDataThreadQueue.Count > 0)
        {
            for (int i = 0; i < worldDataThreadQueue.Count; i++)
            {
                //
                WorldThreadInfo<WorldData> threadInfo = worldDataThreadQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    public WorldData GenerateWorldData()
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

        return new WorldData(noiseMap, colourMap);
    }
}