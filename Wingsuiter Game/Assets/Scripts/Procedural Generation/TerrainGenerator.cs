using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public class TerrainGenerator : MonoBehaviour 
{
    // The threshold distance the player needs to move to trigger a chunk update
    const float playerMoveThresholdForChunkUpdate = 25f;
	const float sqrPlayerMoveThresholdForChunkUpdate = playerMoveThresholdForChunkUpdate * playerMoveThresholdForChunkUpdate;


	public int colliderLODIndex;
	public LODInfo[] detailLevels;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureSettings;

	public Transform player;
	public Material mapMaterial;

	Vector2 playerPosition;
	Vector2 playerPositionOld;

	float meshWorldSize;
	int chunksVisibleInViewDst;

    // Store and manage terrain chunks
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    void Start()
    {
        // Apply texture settings to the material and update the mesh heights
        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        meshWorldSize = meshSettings.meshWorldSize;
        chunksVisibleInViewDst = 10;

        // Update visible chunks based on the player's initial position
        UpdateVisibleChunks();
    }

    void Update()
	{
		playerPosition = new Vector2(player.position.x, player.position.z);

        // If the player has moved since the last update, update the collision meshes of visible chunks
        if (playerPosition != playerPositionOld)
		{
			foreach (TerrainChunk chunk in visibleTerrainChunks)
			{
				chunk.UpdateCollisionMesh();
			}
		}

        // If the player has moved beyond the threshold distance, update the visible chunks
        if ((playerPositionOld - playerPosition).sqrMagnitude > sqrPlayerMoveThresholdForChunkUpdate)
		{
			playerPositionOld = playerPosition;
			UpdateVisibleChunks();
		}
	}

    /// <summary>
    /// Update the visible terrain chunks based on the player's position.
    /// </summary>
    void UpdateVisibleChunks() 
	{
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2> (); // Note: every item is unique in a HashSet

        // Iterate over visible chunks and update them
        for (int i = visibleTerrainChunks.Count-1; i >= 0; i--)
		{
			alreadyUpdatedChunkCoords.Add(visibleTerrainChunks [i].coord);
			visibleTerrainChunks [i].UpdateTerrainChunk();
		}
			
		int currentChunkCoordX = Mathf.RoundToInt(playerPosition.x / meshWorldSize);
		int currentChunkCoordY = Mathf.RoundToInt(playerPosition.y / meshWorldSize);

        // Iterate over the chunks within the view distance to update or load them
        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) 
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) 
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) 
				{
					if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) 
					{
						terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					} 
					else 
					{
						TerrainChunk newChunk = new TerrainChunk (viewedChunkCoord,heightMapSettings,meshSettings, detailLevels, colliderLODIndex, transform, player, mapMaterial);
						terrainChunkDictionary.Add (viewedChunkCoord, newChunk);
						newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
						newChunk.Load ();
					}
				}

			}
		}
	}

    /// <summary>
    /// Handle visibility changes for terrain chunks.
    /// </summary>
    /// <param name="chunk">The chunk where the visibility changed</param>
    /// <param name="isVisible">Whether the chunk is now visible or not</param>
    void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible) 
	{
		if (isVisible) 
		{
			visibleTerrainChunks.Add(chunk);
		} 
		else 
		{
			visibleTerrainChunks.Remove(chunk);
		}
	}

}

/// <summary>
/// This struct stores information about the level of detail (LOD) for the terrain
/// </summary>
[System.Serializable]
public struct LODInfo 
{
	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int lod;
	public float visibleDstThreshold;

    // Property to return the square of the visibility distance threshold
    public float sqrVisibleDstThreshold 
	{
		get 
		{
			return visibleDstThreshold * visibleDstThreshold;
		}
	}
}
