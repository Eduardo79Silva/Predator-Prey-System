using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuadCreator : MonoBehaviour
{
    public float width = 1;
    public float height = 1;

    public float gridSize = 2;

    bool needsUpdate;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            Generate();
        }
    }

    public void Generate()
    {
        if (meshFilter == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshFilter = gameObject.AddComponent<MeshFilter>();

            mesh = new Mesh();
        }


        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();
        var normals = new List<Vector3>();
        Vector3[] normalsSet = new Vector3[4] {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };
        Vector2[] uv = new Vector2[4] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                int elevation = UnityEngine.Random.Range(0, 2);
                float x = i * width;
                float y = j * height;
                Vector3[] quadVertices = new Vector3[4]{
                    new Vector3(x, elevation, y),
                    new Vector3(x + width, elevation, y),
                    new Vector3(x, elevation, y + height),
                    new Vector3(x + width, elevation, y + height)
                };

                int vertIndex = vertices.Count;
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 1);
                triangles.Add(vertIndex);
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 3);
                triangles.Add(vertIndex + 1);

                vertices.AddRange(quadVertices);
                normals.AddRange(normalsSet);
                uvs.AddRange(uv);
            }
        }


        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetTriangles(triangles, 0, true);
        mesh.SetUVs(0, uvs);

        meshFilter.mesh = mesh;
    }

    void OnValidate()
    {
        needsUpdate = true;
    }

}
