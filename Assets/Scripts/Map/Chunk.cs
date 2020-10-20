using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Chunk
{
    private readonly List<Tile> _tiles;
    private readonly int _size;
    private readonly GameObject _chunkObj;

    public Chunk(string name, int size)
    {
#if UNITY_EDITOR
        Assert.IsTrue(size >= 2, "Size must be a minimum of 2.");
        Assert.IsTrue(size % 2 == 0, "Size must be divisible by 2.");
#endif
        _tiles = new List<Tile>(size * size);
        _size = size;
        _chunkObj = new GameObject(name);
    }

    public void Create()
    {
        int halfSize = _size / 2;
        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                _tiles.Add(GenerateTile(x, y));
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
        tile.Initialize(x, y);
        go.transform.parent = _chunkObj.transform;
        return tile;
    }
}
