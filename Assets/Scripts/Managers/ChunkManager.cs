using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager Instance { get; private set; }

    public bool AverageColors;
    public Color DefaultColor;

    private List<Tile> _tilesFlat;
    private Tile[,] _tiles;
    private Material _tileMaterial;
    private int _size;
    private GameObject _chunkObj;

    protected void Awake()
    {
        Instance = this;

        int size = 16;

        _tilesFlat = new List<Tile>(size * size);
        _tiles = new Tile[size+1, size+1];
        _size = size;
        _chunkObj = new GameObject("DefaultChunk");
        _tileMaterial = new Material(Shader.Find("VertexLit"))
        {
            color = DefaultColor
        };

        Create();
    }

    protected void Update()
    {
        if (AverageColors)
        {
            int halfSize = _size / 2;
            for (int x = -halfSize; x <= halfSize; x++)
            {
                for (int y = -halfSize; y <= halfSize; y++)
                {
                    var tile = GetTile(x, y);
                    if (tile != null)
                    {
                        tile.AverageColors();
                    }
                }
            }

            AverageColors = false;
        }

    }

    public void Create()
    {
        int halfSize = _size / 2;
        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                var tile = GenerateTile(x, y);
                _tilesFlat.Add(tile);
                _tiles[x + halfSize, y + halfSize] = tile;
            }
        }
    }

    public void Save()
    {

    }

    private Tile GenerateTile(int x, int y)
    {
        var go = new GameObject($"{x}_{y}");
        var tile = go.AddComponent<Tile>();
        tile.Initialize(x, y, _tileMaterial);
        go.transform.parent = _chunkObj.transform;
        return tile;
    }

    public Tile GetTile(int x, int y)
    {
        int halfSize = _size / 2;
        x += halfSize;
        y += halfSize;

        if (x > _tiles.GetLength(0) - 1 || x < 0)
        {
            return null;
        }

        if (y > _tiles.GetLength(1) - 1 || y < 0)
        {
            return null;
        }

        return _tiles[x, y];
    }
}
