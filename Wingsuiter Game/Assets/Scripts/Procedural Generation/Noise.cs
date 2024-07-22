using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    /// <summary>
    /// https://www.youtube.com/watch?v=MRNFcywkUSA
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity)
    {
        Debug.Log($"WIDTH: {mapWidth}, HEIGHT: {mapHeight}");
        float[,] noiseMap = new float[mapWidth, mapHeight];

        Random.InitState(2);
        // Get a random...
        //System.Random psuedoRandomNumber = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            //float offsetX = psuedoRandomNumber.Next(-100000, 100000);
            //float offsetY = psuedoRandomNumber.Next(-100000, 100000);
            float offsetX = Random.Range(-100000, 100000);
            float offsetY = Random.Range(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Manage the max and min noise of the noise map
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfMapWidth = mapWidth / 2f;
        float halfMapHeight = mapHeight / 2f;

        for (int x = 0; x < mapWidth; x++)
        { 
            for (int y = 0; y < mapHeight; y++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Iterate over each octave (
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfMapWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfMapHeight) / scale * frequency + octaveOffsets[i].y;
                    
                    // https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    //noiseMap[x, y] = perlinValue;
                    //
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight  > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Iterate over the noise map
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Normalise...
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            } 
        }

        return noiseMap;
    }
}
