using UnityEngine;
using System.Linq;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
[CreateAssetMenu()]
public class TextureData : UpdatableData
{
	const int textureSize = 512;
	const TextureFormat textureFormat = TextureFormat.RGB565;

	public Layer[] layers;

	float savedMinHeight;
	float savedMaxHeight;

	/// <summary>
	/// Applies the texture data to the given material.
	/// </summary>
	/// <param name="material">The material to apply the texture data to</param>
	public void ApplyToMaterial(Material material) 
	{
		material.SetInt ("layerCount", layers.Length);
		material.SetColorArray ("baseColours", layers.Select(x => x.tint).ToArray());
		material.SetFloatArray ("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
		material.SetFloatArray ("baseBlends", layers.Select(x => x.blendStrength).ToArray());
		material.SetFloatArray ("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
		material.SetFloatArray ("baseTextureScales", layers.Select(x => x.textureScale).ToArray());

        // Generate a texture array from the textures in the layers and apply it to the material
        Texture2DArray texturesArray = GenerateTextureArray (layers.Select (x => x.texture).ToArray ());
		material.SetTexture("baseTextures", texturesArray);

		UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
	}

	/// <summary>
	/// Updates the mesh height properties in the material.
	/// </summary>
	/// <param name="material">The material to update</param>
	/// <param name="minHeight">The minimum height of the mesh</param>
	/// <param name="maxHeight">The maximum height of the mesh</param>
	public void UpdateMeshHeights(Material material, float minHeight, float maxHeight) 
	{
		savedMinHeight = minHeight;
		savedMaxHeight = maxHeight;

		material.SetFloat("minHeight", minHeight);
		material.SetFloat("maxHeight", maxHeight);
	}

    /// <summary>
    /// Set the pixels of each of the textures in the texture array and apply the changes to the texture array.
    /// </summary>
    /// <param name="textures">Array of textures to be included in the texture array</param>
    /// <returns>A Texture2DArray containing the provided textures</returns>
    Texture2DArray GenerateTextureArray(Texture2D[] textures) 
	{
		Texture2DArray textureArray = new Texture2DArray (textureSize, textureSize, textures.Length, textureFormat, true);
		for (int i = 0; i < textures.Length; i++) 
		{
			textureArray.SetPixels(textures [i].GetPixels(), i);
		}
		textureArray.Apply();
		return textureArray;
	}

	/// <summary>
	/// Class representing a layer of the texture data
	/// </summary>
	[System.Serializable]
	public class Layer 
	{
		public Texture2D texture;
		public Color tint;
		[Range(0,1)]
		public float tintStrength;
		[Range(0,1)]
		public float startHeight;
		[Range(0,1)]
		public float blendStrength;
		public float textureScale;
	}
}
