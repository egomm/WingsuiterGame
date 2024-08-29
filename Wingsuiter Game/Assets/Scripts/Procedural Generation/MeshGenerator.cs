using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public static class MeshGenerator
{
    /// <summary>
    /// Generate the terrain mesh based on the height map, mesh settings, and level of detail.
    /// </summary>
    /// <param name="heightMap">The height map for the terrain</param>
    /// <param name="meshSettings">The settings for the terrain's mesh</param>
    /// <param name="levelOfDetail">The level of detail of the terrain</param>
    /// <returns>The mesh data for the terrain mesh</returns>
    public static MeshData GenerateTerrainMesh(float[,] heightMap, MeshSettings meshSettings, int levelOfDetail)
    {
        int vertexSkip = 1;
        if (levelOfDetail != 0)
        {
            vertexSkip = levelOfDetail * 2;
        }

        int verticesPerLine = meshSettings.numVertsPerLine;
        Vector2 topLeftPosition = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

        // Initalise a new mesh data object
        MeshData meshData = new MeshData(verticesPerLine, vertexSkip, false);
        int[,] vertexIndexMap = new int[verticesPerLine, verticesPerLine];
        int meshVertexIndex = 0;
        int outOfMeshVertexIndex = -1;

        /// Determine the index of each vertex
        for (int y = 0; y < verticesPerLine; y++)
        {
            for (int x = 0; x < verticesPerLine; x++)
            {
                // Check if the vertex is on the border
                bool isBorderVertex = y == 0 || y == verticesPerLine - 1 || x == 0 || x == verticesPerLine - 1;
                // Check if the vertex should be skipped based on the LOD (note: vertices are skipped for less detail)
                bool isSkippedVertex = x > 2 && x < verticesPerLine - 3 && y > 2 && y < verticesPerLine - 3 &&
                                       ((x - 2) % vertexSkip != 0 || (y - 2) % vertexSkip != 0);

                if (isBorderVertex)
                {
                    vertexIndexMap[x, y] = outOfMeshVertexIndex;
                    outOfMeshVertexIndex--;
                }
                else if (!isSkippedVertex)
                {
                    vertexIndexMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
        }

        // Assign positions to vertices and generate the triangles connecting them
        for (int y = 0; y < verticesPerLine; y++)
        {
            for (int x = 0; x < verticesPerLine; x++)
            {
                bool isSkippedVertex = x > 2 && x < verticesPerLine - 3 && y > 2 && y < verticesPerLine - 3 &&
                                       ((x - 2) % vertexSkip != 0 || (y - 2) % vertexSkip != 0);

                if (!isSkippedVertex)
                {
                    // Determine the type of vertex (border, edge, main, connecting)
                    bool isBorderVertex = y == 0 || y == verticesPerLine - 1 || x == 0 || x == verticesPerLine - 1;
                    bool isEdgeVertex = (y == 1 || y == verticesPerLine - 2 || x == 1 || x == verticesPerLine - 2) && !isBorderVertex;
                    bool isMainVertex = (x - 2) % vertexSkip == 0 && (y - 2) % vertexSkip == 0 && !isBorderVertex && !isEdgeVertex;
                    bool isConnectingEdgeVertex = (y == 2 || y == verticesPerLine - 3 || x == 2 || x == verticesPerLine - 3) && !isBorderVertex && !isEdgeVertex && !isMainVertex;

                    int vertexIndex = vertexIndexMap[x, y];
                    Vector2 vertexPercent = new Vector2(x - 1, y - 1) / (verticesPerLine - 3);
                    Vector2 vertexPosition2D = topLeftPosition + new Vector2(vertexPercent.x, -vertexPercent.y) * meshSettings.meshWorldSize;
                    float height = heightMap[x, y];

                    if (isConnectingEdgeVertex)
                    {
                        // Calculate the height for connecting edge vertices by interpolating between neighbouring main vertices
                        bool isVertical = x == 2 || x == verticesPerLine - 3;
                        int distanceToMainVertexA;
                        if (isVertical)
                        {
                            distanceToMainVertexA = (y - 2) % vertexSkip;
                        }
                        else
                        {
                            distanceToMainVertexA = (x - 2) % vertexSkip;
                        }

                        int distanceToMainVertexB = vertexSkip - distanceToMainVertexA;
                        float distancePercent = distanceToMainVertexA / (float)vertexSkip;

                        float heightMainVertexA;
                        if (isVertical)
                        {
                            heightMainVertexA = heightMap[x, y - distanceToMainVertexA];
                        }
                        else
                        {
                            heightMainVertexA = heightMap[x - distanceToMainVertexA, y];
                        }

                        float heightMainVertexB;
                        if (isVertical)
                        {
                            heightMainVertexB = heightMap[x, y + distanceToMainVertexB];
                        }
                        else
                        {
                            heightMainVertexB = heightMap[x + distanceToMainVertexB, y];
                        }

                        height = heightMainVertexA * (1 - distancePercent) + heightMainVertexB * distancePercent;
                    }

                    meshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), vertexPercent, vertexIndex);

                    // Add triangles between vertices to form the mesh surface
                    if (x < verticesPerLine - 1 && y < verticesPerLine - 1 && (!isConnectingEdgeVertex || (x != 2 && y != 2)))
                    {
                        int currentIncrement = 1;
                        if (isMainVertex && x != verticesPerLine - 3 && y != verticesPerLine - 3)
                        {
                            // Adjusts triangle size based on level of detail
                            currentIncrement = vertexSkip;
                        }

                        // This makes a square (a, b, c, d) out of two triangles (a, d, c) and (d, a, b)
                        int a = vertexIndexMap[x, y];
                        int b = vertexIndexMap[x + currentIncrement, y];
                        int c = vertexIndexMap[x, y + currentIncrement];
                        int d = vertexIndexMap[x + currentIncrement, y + currentIncrement];
                        meshData.AddTriangle(a, d, c);
                        meshData.AddTriangle(d, a, b);
                    }
                }
            }
        }

        // Process this mesh data and then return the mesh data
        meshData.ProcessMesh();
        return meshData;
    }
}

public class MeshData
{
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] bakedNormals;

    Vector3[] outOfMeshVertices;
    int[] outOfMeshTriangles;

    int triangleIndex;
    int outOfMeshTriangleIndex;

    bool useFlatShading;

    /// <summary>
    /// This constructor initalises arrays based on the number of vertices and triangles
    /// </summary>
    /// <param name="verticesPerLine">The number of vertices per line</param>
    /// <param name="vertexSkip">Manage the vertices to skip</param>
    /// <param name="useFlatShading">Whether to use flat or smooth shading for the mesh</param>
    public MeshData(int verticesPerLine, int vertexSkip, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;

        int numEdgeVertices = (verticesPerLine - 2) * 4 - 4;
        int numConnectingEdgeVertices = (vertexSkip - 1) * (verticesPerLine - 5) / vertexSkip * 4;
        int numMainVerticesPerLine = (verticesPerLine - 5) / vertexSkip + 1;
        int numMainVertices = numMainVerticesPerLine * numMainVerticesPerLine;

        vertices = new Vector3[numEdgeVertices + numConnectingEdgeVertices + numMainVertices];
        uvs = new Vector2[vertices.Length];

        int numEdgeTriangles = 8 * (verticesPerLine - 4);
        int numMainTriangles = (numMainVerticesPerLine - 1) * (numMainVerticesPerLine - 1) * 2;
        triangles = new int[(numEdgeTriangles + numMainTriangles) * 3];

        outOfMeshVertices = new Vector3[verticesPerLine * 4 - 4];
        outOfMeshTriangles = new int[24 * (verticesPerLine - 2)];
    }

    /// <summary>
    /// Add a vertex at a given position.
    /// </summary>
    /// <param name="vertexPosition">The position of the vertex</param>
    /// <param name="uv">Store horizontal and vertical positions on the texture (ie. a 2D vector)</param>
    /// <param name="vertexIndex">The index of the vertex</param>
    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        if (vertexIndex < 0)
        {
            outOfMeshVertices[-vertexIndex - 1] = vertexPosition;
        }
        else
        {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }
    }

    /// <summary>
    /// Add a triangle to the triangles array with vertices (a, b, c)
    /// </summary>
    /// <param name="a">First vertex of the triangle</param>
    /// <param name="b">Second vertex of the triangle</param>
    /// <param name="c">Third vertex of the triangle</param>
    public void AddTriangle(int a, int b, int c)
    {
        if (a < 0 || b < 0 || c < 0)
        {
            outOfMeshTriangles[outOfMeshTriangleIndex] = a;
            outOfMeshTriangles[outOfMeshTriangleIndex + 1] = b;
            outOfMeshTriangles[outOfMeshTriangleIndex + 2] = c;
            outOfMeshTriangleIndex += 3;
        }
        else
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }
    }

    /// <summary>
    /// Calculate the normals on the mesh
    /// </summary>
    /// <returns></returns>
    Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        int borderTriangleCount = outOfMeshTriangles.Length / 3;
        for (int i = 0; i < borderTriangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = outOfMeshTriangles[normalTriangleIndex];
            int vertexIndexB = outOfMeshTriangles[normalTriangleIndex + 1];
            int vertexIndexC = outOfMeshTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            if (vertexIndexA >= 0)
            {
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0)
            {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0)
            {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    /// <summary>
    /// Calculates the surface normal of a triangle given its vertex indices.
    /// This is done by getting the cross product from the triangle (indexA, indexB, indexC).
    /// </summary>
    Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = (indexA < 0) ? outOfMeshVertices[-indexA - 1] : vertices[indexA];
        Vector3 pointB = (indexB < 0) ? outOfMeshVertices[-indexB - 1] : vertices[indexB];
        Vector3 pointC = (indexC < 0) ? outOfMeshVertices[-indexC - 1] : vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    /// <summary>
    /// Process the mesh
    /// </summary>
    public void ProcessMesh()
    {
        if (useFlatShading)
        {
            FlatShading();
        }
        else
        {
            BakeNormals();
        }
    }

    /// <summary>
    /// Calculates normals for smooth shading by averaging triangle normals
    /// </summary>
    void BakeNormals()
    {
        bakedNormals = CalculateNormals();
    }

    /// <summary>
    /// Applies flat shading by creating duplicate vertices for each triangle, so each triangle has its own unique vertices
    /// </summary>
    void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUVs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUVs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUVs;
    }

    /// <summary>
    /// Create the mesh with the data in this class.
    /// </summary>
    /// <returns>The created mesh</returns>
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uvs
        };
        if (useFlatShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.normals = bakedNormals;
        }

        return mesh;
    }
}
