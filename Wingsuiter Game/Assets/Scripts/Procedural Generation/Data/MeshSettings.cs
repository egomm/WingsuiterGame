using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
[CreateAssetMenu()]
public class MeshSettings : UpdatableData
{
	// Note LOD: level of detail
	public const int numSupportedLODs = 5;
	public const int numSupportedChunkSizes = 9;
	public const int numSupportedFlatshadedChunkSizes = 3;
	public static readonly int[] supportedChunkSizes = {48,72,96,120,144,168,192,216,240};
	
	public float meshScale = 2.5f;

	[Range(0,numSupportedChunkSizes-1)]
	public int chunkSizeIndex;
	[Range(0,numSupportedFlatshadedChunkSizes-1)]
	public int flatshadedChunkSizeIndex;


	// Property for calculating the number of vertices per line given the current chunk size
	public int numVertsPerLine 
	{
		get 
		{
			return supportedChunkSizes [chunkSizeIndex] + 5;
		}
	}

	// Property to calculate the world size based on the number of vertices per line and the mesh scale
	public float meshWorldSize 
	{
		get 
		{
			return (numVertsPerLine - 3) * meshScale;
		}
	}


}
