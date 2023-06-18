namespace RTS;

public class Chunk
{
    TileMap tileMap;

    public Chunk(int chunkX, int chunkY, TileMap tileMap, Noise noise)
    {
        this.tileMap = tileMap;

        for (int x = 0; x < World.ChunkSize; x++)
        {
            for (int y = 0; y < World.ChunkSize; y++)
            {
                var globalX = (chunkX * World.ChunkSize) + x;
                var globalY = (chunkY * World.ChunkSize) + y;

                string type;

                if (noise.GetNoise2D(globalX, globalY) < 0.3f)
                    type = "grass_1";
                else
                    type = "grass_2";

                SetCell(globalX, globalY, World.Atlas[type]);
            }
        }
    }

    void SetCell(int x, int y, Vector2I type) =>
        tileMap.SetCell(0, new Vector2I(x, y), 0, type);
}
