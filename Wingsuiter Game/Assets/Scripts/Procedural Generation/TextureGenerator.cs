using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public static class TextureGenerator 
{
    /// <summary>
    /// Generate a 2D texture from a given colour map
    /// </summary>
    /// <param name="colourMap">An array of colours representing the texture</param>
    /// <param name="width">The width of the texture</param>
    /// <param name="height">The height of the texture</param>
    /// <returns>A Texture2D object created from the colour map</returns>
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) 
	{
		Texture2D texture = new Texture2D(width, height);

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.SetPixels(colourMap);
		texture.Apply();

		return texture;
	}

    /// <summary>
    /// Generates a Texture2D from a height map by mapping height values to greyscale colours.
    /// </summary>
    /// <param name="heightMap">The height map data used for generating the texture</param>
    /// <returns>A Texture2D object representing the height map as a greyscale</returns>
	public static Texture2D TextureFromHeightMap(HeightMap heightMap) 
	{
        // Get the dimensions of the height map
        int width = heightMap.values.GetLength(0);
		int height = heightMap.values.GetLength(1);

		Color[] colourMap = new Color[width * height];

		for (int y = 0; y < height; y++) 
		{
			for (int x = 0; x < width; x++) 
			{
                // Map the height value to a greyscale colour between black and white
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(heightMap.minValue, heightMap.maxValue, heightMap.values[x, y]));
            }
		}

        // Generate and return a texture from the colour map
        return TextureFromColourMap (colourMap, width, height);
	}
}