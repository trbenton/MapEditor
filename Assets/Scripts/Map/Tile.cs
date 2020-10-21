using UnityEngine;

public class Tile : MonoBehaviour
{
    private Mesh _mesh;

    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private MeshCollider _collider;

    public void Initialize(int x, int y)
    {
        _mesh = CreateQuadMesh();

        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.material = new Material(Shader.Find("Custom/TileUnlit"))
        {
            color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))
        };

        _filter = gameObject.AddComponent<MeshFilter>();
        _filter.mesh = _mesh;

        _collider = gameObject.AddComponent<MeshCollider>();
        _collider.sharedMesh = _mesh;

        gameObject.transform.position = new Vector3(x * 1.0f, 0.0f, y * 1.0f);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        gameObject.layer = (int)EditorLayers.MapTile;
    }

    public Vector3 GetClosestVertex(Vector3 point)
    {
        var vertex = Vector3.zero;
        float minDistance = float.MaxValue;
        var verts = _filter.mesh.vertices;
        var localToWorld = transform.localToWorldMatrix;
        for (int i = 0; i < verts.Length; i++)
        {
            var vert = localToWorld.MultiplyPoint3x4(verts[i]);
            float distance = (point - vert).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                vertex = vert;
            }
        }
        return vertex;
    }

    private Mesh CreateQuadMesh()
    {
        var mesh = new Mesh();

        var vertices = new[]
        {
            new Vector3(-0.5f, 0.0f, -0.5f),
            new Vector3(0.5f, 0.0f, -0.5f),
            new Vector3(-0.5f, 0.0f, 0.5f),
            new Vector3(0.5f, 0.0f, 0.5f)
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
