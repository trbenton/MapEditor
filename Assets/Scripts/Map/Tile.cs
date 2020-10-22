using UnityEngine;

public class Tile : MonoBehaviour
{
    private Mesh _mesh;

    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private MeshCollider _collider;

    public void Initialize(Material material, int x, int y)
    {
        _mesh = CreateQuadMesh();

        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.material = material;

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

    public void Raise(Vector3 position)
    {
        ModifyNeighboringTiles(position, 0.1f);
    }

    public void Lower(Vector3 position)
    {
        ModifyNeighboringTiles(position, -0.1f);
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    private void ModifyNeighboringTiles(Vector3 position, float mod)
    {
        const int layerMask = (1 << (int)EditorLayers.MapTile);

        var results = Physics.OverlapSphere(position, 0.25f, layerMask);
        foreach (var res in results)
        {
            var go = res.gameObject;
            var tile = go.GetComponent<Tile>();
            tile.ModifyClosestVertex(position, mod);
        }
    }

    private void ModifyClosestVertex(Vector3 position, float mod)
    {
        int pos = 0;
        float minDistance = float.MaxValue;
        var verts = _filter.mesh.vertices;
        var localToWorld = transform.localToWorldMatrix;
        for (int i = 0; i < verts.Length; i++)
        {
            var vert = localToWorld.MultiplyPoint3x4(verts[i]);
            float distance = (position - vert).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                pos = i;
            }
        }

        verts[pos] = new Vector3(verts[pos].x, verts[pos].y + mod, verts[pos].z);
        _filter.mesh.SetVertices(verts);
        _filter.mesh.RecalculateBounds();
        _collider.sharedMesh = _filter.mesh;
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
