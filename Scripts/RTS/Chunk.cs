using Godot;

namespace RTS;

public class Chunk
{
    TileMap tileMap;

    public Chunk(int chunkX, int chunkY, TileMap tileMap, Noise noise, bool generate = true)
    {
        this.tileMap = tileMap;

        if (generate) 
            this.GenerateChunk(chunkX, chunkY, tileMap, noise);
    }

    void GenerateChunk(int chunkX, int chunkY, TileMap tileMap, Noise noise)
    {
        for (int x = 0; x < World.ChunkSize; x++)
            for (int y = 0; y < World.ChunkSize; y++)
                GenerateTile(chunkX, chunkY, x, y, tileMap, noise);
    }

    void GenerateTile(int chunkX, int chunkY, int x, int y, TileMap tileMap, Noise noise)
    {
        var globalX = (chunkX * World.ChunkSize) + x;
        var globalY = (chunkY * World.ChunkSize) + y;

        string type = "";
        var currentNoise = noise.GetNoise2D(globalX, globalY);

        foreach (var atlasValue in World.AtlasGrass)
        {
            if (currentNoise < atlasValue.Value.Weight)
            {
                type = atlasValue.Key;
                break;
            }
        }

        SetCell(globalX, globalY, type, !string.IsNullOrWhiteSpace(type) ?
            World.AtlasGrass[type].TilePosition :
            World.AtlasGrass.First().Value.TilePosition);
    }

    void SetCell(int x, int y, string typeName, Vector2I type)
    {
        if (typeName != "empty")  tileMap.SetCell(0, new Vector2I(x, y), 0, type);
    }
}
