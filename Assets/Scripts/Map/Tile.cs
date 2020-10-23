using System;
using UnityEngine;
using Random = System.Random;

public class Tile : MonoBehaviour
{
    private int _x;
    private int _y;

    private Mesh _mesh;
    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private MeshCollider _collider;
    private Color _color;
    private Color _averagedColor;

    public void Initialize(int x, int y, Material material)
    {
        _x = x;
        _y = y;

        _mesh = CreateQuadMesh();

        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.material = material;
        _color = material.color;

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
        _color = color;
    }

    public void SetAveragedColor(Color color)
    {
        _renderer.material.color = color;
        _averagedColor = color;
    }

    public Color GetColor()
    {
        return _color;
    }

    public Color GetAveragedColor()
    {
        return _averagedColor;
    }

    public void AverageColors()
    {
        int totalCount = 0;
        float r = 0.0f;
        float g = 0.0f;
        float b = 0.0f;

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                var tile = ChunkManager.Instance.GetTile(_x + x, _y + y);
                if (tile == null)
                {
                    continue;
                }

                int distance = Mathf.RoundToInt((Vector2.zero - new Vector2(x, y)).magnitude);
                int weight = Mathf.RoundToInt(Mathf.Pow(2, distance));

                var tileColor = tile.GetColor();
                Color.RGBToHSV(tileColor, out var hue, out var saturation, out var brightness);

                var random = new Random((_x + x).GetHashCode() ^ (_y + y).GetHashCode());
                hue += random.Next(-16, 16) / 100.0f;
                tileColor = Color.HSVToRGB(hue, saturation, brightness);

                r += (tileColor.r * tileColor.r) * weight;
                g += (tileColor.g * tileColor.g) * weight;
                b += (tileColor.b * tileColor.b) * weight;
                totalCount += weight;
            }
        }

        SetAveragedColor(new Color(Mathf.Sqrt(r / totalCount), Mathf.Sqrt(g / totalCount), Mathf.Sqrt(b / totalCount)));
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
