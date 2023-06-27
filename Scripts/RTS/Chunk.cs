namespace RTS;

public class Chunk
{
    public bool Generated { get; set; }

    public Chunk(int chunkX, int chunkY)
    {
        Generated = true;

        foreach (var layer in World.TileLayers)
        {
            var tileMap = layer.TileMap;

            for (int x = 0; x < World.ChunkSize; x++)
            {
                for (int y = 0; y < World.ChunkSize; y++)
                {
                    // Obtain tile atlas coords
                    var tileAtlasCoords = Vector2I.Zero;

                    foreach (var tile in layer.TileData)
                    {
                        if (layer.FNL.GetNoise2D(x, y) < tile.Value.Weight)
                        {
                            tileAtlasCoords = tile.Value.Atlas;
                            break;
                        }
                    }

                    GD.Print($"Set cell at ({x}, {y}) using atlas coords {tileAtlasCoords}");
                    SetCell(tileMap, x, y, tileAtlasCoords);
                }
            }
        }
    }

    void SetCell(TileMap tileMap, int x, int y, Vector2I atlasCoords) =>
        tileMap.SetCell(0, new Vector2I(x, y), 0, atlasCoords);
}
