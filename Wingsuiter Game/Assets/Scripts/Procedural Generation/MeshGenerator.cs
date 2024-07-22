using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        //
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        // Fix lighting
        mesh.RecalculateNormals();

        return mesh;
    }
}

public class MeshGenerator : MonoBehaviour
{
    public void AddTriangle(int vertA, int vertB, int vertC)
    {

    }

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        Debug.Log($"Width: {width}, Height: {height}");
        // Left
        float topLeftX = (width - 1) / -2f;
        // Up
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //
                float halfWidth = (width - 1) / 2f;
                float halfHeight = (height - 1) / 2f;
                meshData.vertices[vertexIndex] = new Vector3(x - halfWidth, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, y - halfHeight);
                //meshData.uvs[vertexIndex] = new Vector2(1f - (float) x / width, 1f - (float) y / height);
                meshData.uvs[vertexIndex] = new Vector2((float)x / width, (float)y / height);

                // Ignore right & bottom edge vertices
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}
