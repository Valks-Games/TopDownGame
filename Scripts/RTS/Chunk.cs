namespace RTS;

public class Chunk
{
    TileMap tileMap;

    public Chunk(int chunkX, int chunkY, TileMap tileMap, Noise noise, bool generate = true)
    {
        this.tileMap = tileMap;

        if (generate) 
            this.GenerateWorld(chunkX, chunkY, tileMap, noise);
    }

    void GenerateWorld(int chunkX, int chunkY, TileMap tileMap, Noise noise)
    {
        for (int x = 0; x < World.ChunkSize; x++)
        {
            for (int y = 0; y < World.ChunkSize; y++)
            {
                var globalX = (chunkX * World.ChunkSize) + x;
                var globalY = (chunkY * World.ChunkSize) + y;

                string type = "";
                var currentNoise = noise.GetNoise2D(globalX, globalY);
                var name = ((string) (this.tileMap.Name)).ToLower();
                var limitedAtlas = World.Atlas.Where(pair => pair.Key.ToLower().Contains(name)).ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (var atlasValue in limitedAtlas)
                {
                    if (currentNoise < atlasValue.Value.Weight)
                    {
                        type = atlasValue.Key;
                        break;
                    }
                }

                if (type != string.Empty) 
                    SetCell(globalX, globalY, World.Atlas[type].TilePosition);

                // GD.PrintErr not printing to console in editor?
                else GD.PrintErr("No type found for noise: " + currentNoise);
            }
        }
    }

    void SetCell(int x, int y, Vector2I type) =>
        tileMap.SetCell(0, new Vector2I(x, y), 0, type);
}
