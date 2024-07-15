using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static int worldWidth = 100;
    public static int worldHeight = 100;
    public static float noiseScale = 0.3f;

    public static void GenerateWorld()
    {
        // Get the noise map from the noise class
        float[,] noiseMap = Noise.GenerateNoiseMap(worldWidth, worldHeight, noiseScale);

        WorldDisplay display = FindObjectOfType<WorldDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
