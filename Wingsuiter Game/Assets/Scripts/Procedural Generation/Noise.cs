using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        { 
            for (int y = 0; y < mapHeight; y++)
            {
                float currentX = x / scale;
                float currentY = y / scale;

                // https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html
                float perlinValue = Mathf.PerlinNoise(currentX, currentY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
