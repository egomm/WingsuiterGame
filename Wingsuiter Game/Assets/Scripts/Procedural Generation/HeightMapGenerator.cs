using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public static class HeightMapGenerator {
    /// <summary>
    /// Generate a height map based on the height map settings.
    /// </summary>
    /// <param name="width">The width of the height map</param>
    /// <param name="height">The height of the height map</param>
    /// <param name="settings">The settings for generating the height map, containing noise settings and the height curve</param>
    /// <param name="sampleCentre">The central point from which noise mapping will start</param>
    /// <returns>A height map object containing the generated values and their minimum and maximum heights</returns>
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre) 
	{
        // Generate a noise map using the provided settings and sample center
        float[,] values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCentre);

        // Create a thread-safe copy of the height curve
        AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

		float minValue = float.MaxValue;
		float maxValue = float.MinValue;

        // Apply the height curve and multiplier to each value in the noise map (which is a 2D array)
        for (int i = 0; i < width; i++) 
		{
			for (int j = 0; j < height; j++) 
			{
				values [i, j] *= heightCurve_threadsafe.Evaluate(values [i, j]) * settings.heightMultiplier;

                // Track the minimum and maximum values in the height map
                if (values [i, j] > maxValue) 
				{
					maxValue = values [i, j];
				}
				if (values [i, j] < minValue) 
				{
					minValue = values [i, j];
				}
			}
		}

		return new HeightMap (values, minValue, maxValue);
	}

}

/// <summary>
/// Struct representing a height map, including its values and the minimum and maximum heights.
/// </summary>
public struct HeightMap 
{
	public readonly float[,] values;
	public readonly float minValue;
	public readonly float maxValue;

	public HeightMap (float[,] values, float minValue, float maxValue)
	{
		this.values = values;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}

