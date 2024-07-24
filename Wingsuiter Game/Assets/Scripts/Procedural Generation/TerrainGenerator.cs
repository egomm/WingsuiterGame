using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainGenerator : MonoBehaviour
{
    public const float maxView = 2500;
    public Transform player;

    public static Vector2 playerPosition;
    int chunkSize;
    int chunksVisible;

    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> activeChunks = new List<TerrainChunk>();

    private void Start()
    {
        // Each chunk is 240 x 240
        chunkSize = WorldGenerator.mapChunkSize - 1;
        chunksVisible = (int)maxView / chunkSize;
    }

    private void Update()
    {
        playerPosition = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunks.Count; i++)
        {
            activeChunks[i].SetVisible(false);
        }
        activeChunks.Clear();

        int currentChunkCoordX = (int)playerPosition.x / chunkSize;
        int currentChunkCoordY = (int)playerPosition.y / chunkSize;

        for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
        {
            for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                // 
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
                    terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        public Vector2 position;
        Bounds bounds;
        
        public TerrainChunk(Vector2 coordinate, int size, Transform parent)
        {
            position = coordinate * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 position3D = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = position3D;
            // Divided by 10 since the plane is 10 units by default
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            SetVisible(false);
        }

        public void UpdateChunk()
        {
            float distanceToNearest = Mathf.Sqrt(bounds.SqrDistance(position));
            bool visible = distanceToNearest <= maxView;
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
