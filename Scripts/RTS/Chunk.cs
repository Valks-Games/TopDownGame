namespace RTS;

public class Chunk
{
    public Chunk(int chunkX, int chunkY)
    {
        this.GenerateChunk(chunkX, chunkY);
    }

    void GenerateChunk(int chunkX, int chunkY)
    {
        foreach (var atlas in World.Atlases)
            for (int x = 0; x < World.ChunkSize; x++)
                for (int y = 0; y < World.ChunkSize; y++)
                    GenerateTile(chunkX, chunkY, x, y, atlas);
    }

    void GenerateTile(int chunkX, int chunkY, int x, int y, Atlas atlas)
    {
        var globalX = (chunkX * World.ChunkSize) + x;
        var globalY = (chunkY * World.ChunkSize) + y;

        string type = "";
        var currentNoise = atlas.FNL.GetNoise2D(globalX, globalY);

        foreach (var atlasValue in atlas.TileData)
        {
            if (currentNoise < atlasValue.Value.Weight)
            {
                type = atlasValue.Key;
                break;
            }
        }

        SetCell(
            atlas.TileMap, 
            type,
            globalX, 
            globalY, 
            atlas.TileData[type].TilePosition);
    }

    void SetCell(TileMap tileMap, string typeName, int x, int y, Vector2I atlasPos)
    {
        if (typeName != "empty")
            tileMap.SetCell(0, new Vector2I(x, y), 0, atlasPos);
    }
}
