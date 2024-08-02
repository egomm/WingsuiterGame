using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public const float maxView = 1000;
    public Transform player;

    public static Vector2 playerPosition;
    static WorldGenerator worldGenerator;
    int chunkSize;
    int chunksVisible;

    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> activeChunks = new List<TerrainChunk>();

    private void Start()
    {
        worldGenerator = FindObjectOfType<WorldGenerator>();
        chunkSize = WorldGenerator.mapChunkSize - 1;
        chunksVisible = Mathf.CeilToInt(maxView / chunkSize);
    }

    private void Update()
    {
        playerPosition = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        foreach (var chunk in activeChunks)
        {
            chunk.SetVisible(false);
        }
        activeChunks.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
        {
            for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunks.ContainsKey(viewedChunkCoord))
                {
                    terrainChunks[viewedChunkCoord].UpdateChunk();
                    if (terrainChunks[viewedChunkCoord].IsVisible())
                    {
                        activeChunks.Add(terrainChunks[viewedChunkCoord]);
                    }
                }
                else
                {
                    TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, chunkSize, transform);
                    terrainChunks.Add(viewedChunkCoord, newChunk);
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer renderer;
        MeshFilter filter;

        public TerrainChunk(Vector2 coord, int size, Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 position3D = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            renderer = meshObject.AddComponent<MeshRenderer>();
            filter = meshObject.AddComponent<MeshFilter>();

            meshObject.transform.position = position3D;
            meshObject.transform.localScale = Vector3.one; //* size / 10f;
            meshObject.transform.parent = parent;
            SetVisible(false);

            worldGenerator.RequestWorldData(OnWorldDataReceived);
        }

        void OnWorldDataReceived(WorldData worldData)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(worldData.heightMap, WorldGenerator.meshHeightMultiplier, worldGenerator.meshHeightCurve, WorldGenerator.detail);
            filter.mesh = meshData.CreateMesh();
            renderer.material.mainTexture = TextureGenerator.TextureFromHeightMap(worldData.heightMap);
            SetVisible(true);
        }

        public void UpdateChunk()
        {
            float distanceToNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = distanceToNearestEdge <= maxView;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
}
