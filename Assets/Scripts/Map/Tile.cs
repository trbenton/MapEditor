using UnityEngine;

public class Tile : MonoBehaviour
{
    private Mesh _mesh;

    public void Initialize(int x, int y)
    {
        _mesh = CreateQuadMesh();

        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Custom/TileUnlit"));
        meshRenderer.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = _mesh;

        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = _mesh;

        gameObject.transform.position = new Vector3(x * 1, 0.0f, y * 1);
        gameObject.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }

    private Mesh CreateQuadMesh()
    {
        var mesh = new Mesh();

        var vertices = new[]
        {
            new Vector3(-0.5f, -0.5f, 0.0f),
            new Vector3(0.5f, -0.5f, 0.0f),
            new Vector3(-0.5f, 0.5f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f)
        };
        mesh.vertices = vertices;

        var tris = new[]
        {
            0,
            3,
            1,
            3,
            0,
            2
        };
        mesh.triangles = tris;

        var normals = new[]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        var uv = new[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f)
        };
        mesh.uv = uv;

        return mesh;
    }
}
