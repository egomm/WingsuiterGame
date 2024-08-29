using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public static class Noise 
{
    /// <summary>
    /// Generates a 2D noise map using Perlin noise, based on the provided settings
    /// </summary>
    /// <param name="mapWidth">Width of the noise map</param>
    /// <param name="mapHeight">Height of the noise map</param>
    /// <param name="settings">Noise settings, including scale, octaves, persistence (how much octaves are weighted), and lacunarity (change in frequency between octaves)</param>
    /// <param name="sampleCentre">The centre point of the noise sample</param>
    /// <returns>A 2D float array representing the noise map</returns>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		// Get the random number corresponding to the seed (this makes it so that two worlds with the same seed are the same!)
		System.Random random = new System.Random(DataManager.currentWorld.seed);

		Vector2[] octaveOffsets = new Vector2[settings.octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

        // Iterative through each octave to calculate offsets and the maximum possible height
        for (int i = 0; i < settings.octaves; i++) {
            // Calculate random offsets for the x and y axes based on the seed and settings
            float offsetX = random.Next(-100000, 100000) + settings.offset.x + sampleCentre.x;
			float offsetY = random.Next(-100000, 100000) - settings.offset.y - sampleCentre.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

        // Iterate through each point in the noise map
        for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
                // Reset amplitude and frequency for each point
                amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

                // Iterate through each octave to calculate the noise value at this point
                for (int i = 0; i < settings.octaves; i++) {
					float sampleX = (x-halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
					float sampleY = (y-halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

                    // Generate Perlin noise value for the sample positions and scale it to range [-1, 1]
                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= settings.persistance;
					frequency *= settings.lacunarity;
				}

                // Track the maximum and minimum noise heights found
                if (noiseHeight > maxLocalNoiseHeight) {
					maxLocalNoiseHeight = noiseHeight;
				} 
				if (noiseHeight < minLocalNoiseHeight) {
					minLocalNoiseHeight = noiseHeight;
				}
                // Store the calculated noise height in the noise map
                noiseMap[x, y] = noiseHeight;

                // Normalise the noise height to a range of [0, 1] based on the max possible height
                float normalisedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight / 0.9f);
				noiseMap [x, y] = Mathf.Clamp (normalisedHeight, 0, int.MaxValue);
			}
		}

		return noiseMap;
	}

}

/// <summary>
/// A class to hold settings for generating noise, including scale, octaves, persistence, and lacunarity.
/// </summary>
[System.Serializable]
public class NoiseSettings 
{
	public float scale = 50;

	public int octaves = 6;
	[Range(0,1)]
	public float persistance =.6f;
	public float lacunarity = 2;

	public Vector2 offset;

    /// <summary>
    /// Ensures that the noise settings have valid values to prevent errors during noise generation.
    /// </summary>
    public void ValidateValues() 
	{
		scale = Mathf.Max (scale, 0.01f);
		octaves = Mathf.Max (octaves, 1);
		lacunarity = Mathf.Max (lacunarity, 1);
		persistance = Mathf.Clamp01 (persistance);
	}
}