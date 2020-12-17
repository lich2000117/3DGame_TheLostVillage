using System;
using System.Collections;
using UnityEngine;
// using System.IO;

public class CreateMesh : MonoBehaviour
{

    public int sideLen;
    public float size;
    public int num_Row; 
    public int num_Col;
    Vector3[] vertices;
    int num_Vert;

    void Awake()
    {
        // initial the size of plane if user didn't modify them
        if (sideLen == 0) {
            sideLen = 128;
        }

        if (size == 0) {
            size = 5000.0f;
        }

        num_Vert = (sideLen + 1) * (sideLen + 1);
        num_Row = sideLen + 1; 
        num_Col = num_Row;
        
        MeshFilter planeMesh = this.gameObject.AddComponent<MeshFilter>();
        planeMesh.mesh = this.CreateTerrain();
    }

    Mesh CreateTerrain()
    {
        // initialize the mesh and its 3 necessary properties
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[num_Vert];
        Vector2[] uvs = new Vector2[num_Vert];
        int[] triangles = new int[sideLen * sideLen * 2 * 3];

        // initialize some value for calculation conveniency
        int triangles_Vertices = 0;
        float halfSize = size * 0.5f;
        float divisionSize = size / sideLen;
        

        // use double loop to access each square and to assign the 3 properties' values on triangles
        for(int row = 0; row <= sideLen; row++)
        {
            for(int col = 0; col <= sideLen; col++)
            {
                // assign each vertice and uv
                vertices[row * (sideLen + 1) + col] = new Vector3(-halfSize + col * divisionSize, 0.0f, halfSize - row * divisionSize);
                uvs[row * (sideLen + 1) + col] = new Vector2((float)row / sideLen, (float)col / sideLen);
                   
                // make sure that row and col are not reached the boundary
                if (row < sideLen && col < sideLen)
                {
                    // assign the 2 triangles in 1 square, which is in posision (row, col)
                    triangles[triangles_Vertices++] = row * (num_Row) + col;
                    triangles[triangles_Vertices++] = (row + 1) * (num_Row) + (col + 1);
                    triangles[triangles_Vertices++] = (row + 1) * (num_Row) + col;
                    
                    triangles[triangles_Vertices++] = row * (num_Row) + col;
                    triangles[triangles_Vertices++] = row * (num_Row) + (col + 1);
                    triangles[triangles_Vertices++] = (row + 1) * (num_Row) + (col + 1);
                }
            }
        }

        // give properties to mesh after calculation
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;

    }
    
}