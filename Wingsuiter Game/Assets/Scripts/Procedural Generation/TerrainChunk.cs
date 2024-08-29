using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public class TerrainChunk
{

    const float colliderGenerationDistanceThreshold = 5;
    // Event to notify when the visibility of the terrain chunk changes
    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
    public Vector2 coord;

    GameObject meshObject;
    Vector2 sampleCentre;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    LODInfo[] detailLevels;
    LODMesh[] lodMeshes;
    int colliderLODIndex;

    HeightMap heightMap;
    bool heightMapReceived;
    int previousLODIndex = -1;
    bool hasSetCollider;
    // Maximum viewing distance for this chunk (ie. player has to be less than 10,000 metres away)
    float maxViewDst = 10000f;

    HeightMapSettings heightMapSettings;
    MeshSettings meshSettings;
    Transform viewer;

    /// <summary>
    /// Constructor to initalise a TerrainChunk instance.
    /// </summary>
    /// <param name="coord">Coordinates of the terrain chunk</param>
    /// <param name="heightMapSettings">Settings for height map generation</param>
    /// <param name="meshSettings">Settings for mesh generation</param>
    /// <param name="detailLevels">Array of LOD information</param>
    /// <param name="colliderLODIndex">Index of the LOD level for collider generation</param>
    /// <param name="parent">Parent transform to attach the terrain chunk to</param>
    /// <param name="viewer">The transform of the viewer (player)</param>
    /// <param name="material">Material to apply to the terrain mesh</param>
    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material)
    {
        this.coord = coord;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.viewer = viewer;

        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Vector2 position = coord * meshSettings.meshWorldSize;
        bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);


        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;

        meshObject.transform.position = new Vector3(position.x, 0, position.y);
        meshObject.transform.parent = parent;

        // Initially set the terrain chunk to be invisible
        SetVisible(false);

        // Initalise LOD meshes for each level of detail
        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < detailLevels.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;
            if (i == colliderLODIndex)
            {
                lodMeshes[i].updateCallback += UpdateCollisionMesh;
            }
        }

    }

    /// <summary>
    /// Initiates the loading of the height map for this terrain chunk.
    /// </summary>
    public void Load()
    {
        // Request height map data asynchronously
        ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
    }

    /// <summary>
    /// Callback method when the height map data has been received.
    /// </summary>
    /// <param name="heightMapObject">The received height map data</param>
    void OnHeightMapReceived(object heightMapObject)
    {
        this.heightMap = (HeightMap)heightMapObject;
        heightMapReceived = true;

        UpdateTerrainChunk();
    }

    // Property to get the viewer's (player's) current 2D position (X, Z)
    Vector2 viewerPosition
    {
        get
        {
            return new Vector2(viewer.position.x, viewer.position.z);
        }
    }

    /// <summary>
    /// Updates the terrain chunk, adjusting the LOD based on the viewer's distance.
    /// </summary>
    public void UpdateTerrainChunk()
    {
        if (heightMapReceived)
        {
            // Calculate the distance from the viewer (player) to the nearest edge of the chunk
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            bool wasVisible = IsVisible();
            bool visible = viewerDstFromNearestEdge <= maxViewDst;

            if (visible)
            {
                int lodIndex = 0;

                // Determine the appropriate LOD level based on the viewer's distance
                // Note: when the player is further away, the chunk should be less detailed so that there is less lag
                for (int i = 0; i < detailLevels.Length - 1; i++)
                {
                    if (viewerDstFromNearestEdge > 10000)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                // Update the mesh if the LOD index has changed
                if (lodIndex != previousLODIndex)
                {
                    LODMesh lodMesh = lodMeshes[lodIndex];
                    if (lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.hasRequestedMesh)
                    {
                        lodMesh.RequestMesh(heightMap, meshSettings);
                    }
                }


            }

            // Notify listeners if the visibility of the chunk has changed
            if (wasVisible != visible)
            {
                SetVisible(visible);
                if (onVisibilityChanged != null)
                {
                    onVisibilityChanged(this, visible);
                }
            }
        }
    }

    /// <summary>
    /// Updates the collision mesh for this terrain chunk (ie. to detect collisions).
    /// </summary>
    public void UpdateCollisionMesh()
    {
        if (!hasSetCollider)
        {
            float sqrDstFromViewerToEdge = bounds.SqrDistance(viewerPosition);

            // Request the collision mesh if the viewer (player) is within the visible distance threshold
            if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold)
            {
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
                {
                    lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
                }
            }

            // Set the collider if the viewer (player) is close enough to the chunk
            if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold)
            {
                if (lodMeshes[colliderLODIndex].hasMesh)
                {
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                    hasSetCollider = true;
                }
            }
        }
    }

    /// <summary>
    /// Sets the visibility of the terrain chunk.
    /// </summary>
    /// <param name="visible">Whether the chunk should be visible or not</param>
    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    /// <summary>
    /// Checks if the terrain chunk is currently visible.
    /// </summary>
    /// <returns>True if the chunk is visible, otherwise false</returns>
    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }

}

/// <summary>
/// This class represents a mesh at a specific level of detail (LOD).
/// This handles the mesh data for a particular LOD and requests mesh generation as needed.
/// </summary>
class LODMesh
{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    // Event to trigger when the mesh is updated
    public event System.Action updateCallback;

    /// <summary>
    /// Constructor for LODMesh that initalises the level of detail.
    /// </summary>
    /// <param name="lod">Level of detail to be used for this mesh</param>
    public LODMesh(int lod)
    {
        this.lod = lod;
    }

    /// <summary>
    /// Callback function that is invoked when the mesh data is received.
    /// Converts the received data into a Mesh object and triggers the update callback.
    /// </summary>
    /// <param name="meshDataObject">The object containing mesh data</param>
    void OnMeshDataReceived(object meshDataObject)
    {
        mesh = ((MeshData)meshDataObject).CreateMesh();
        hasMesh = true;

        updateCallback();
    }

    /// <summary>
    /// Requests the mesh to be generated for this LOD.
    /// The mesh generation is performed asynchronously.
    /// </summary>
    /// <param name="heightMap">Height map data used for terrain generation</param>
    /// <param name="meshSettings">Settings for mesh generation</param>
    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
    {
        hasRequestedMesh = true;
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
    }

}