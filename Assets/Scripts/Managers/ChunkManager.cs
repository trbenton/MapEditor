using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    protected void Awake()
    {
        var chunk = new Chunk("DefaultChunk", 16);
        chunk.Create();
    }
}
