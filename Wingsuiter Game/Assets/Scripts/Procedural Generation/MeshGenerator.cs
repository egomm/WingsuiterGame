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

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int detail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        // Left
        float topLeftX = (width - 1) / -2f;
        // Up
        float topLeftZ = (height - 1) / 2f;

        int meshIncrement = (detail <= 0) ? 1 : detail * 2;
        int verticesPerLine = (width - 1) / meshIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y += meshIncrement)
        {
            for (int x = 0; x < width; x += meshIncrement)
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
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}
