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
                    var foundAtlasCoords = false;

                    var globalX = (chunkX * World.ChunkSize) + x;
                    var globalY = (chunkY * World.ChunkSize) + y;

                    foreach (var tile in layer.TileData)
                    {
                        if (layer.FNL.GetNoise2D(globalX, globalY) < tile.Value.Weight)
                        {
                            tileAtlasCoords = tile.Value.Atlas;
                            foundAtlasCoords = true;
                            break;
                        }
                    }

                    if (!foundAtlasCoords)
                    {
                        // This should not happen, if it does happen then something
                        // is not working right?
                        Logger.LogWarning("Did not find atlas coords for tile");
                    }

                    //Logger.Log($"Set cell at ({x}, {y}) using atlas coords {tileAtlasCoords}");
                    SetCell(tileMap, globalX, globalY, tileAtlasCoords);
                }
            }
        }
    }

    void SetCell(TileMap tileMap, int x, int y, Vector2I atlasCoords) =>
        tileMap.SetCell(0, new Vector2I(x, y), 0, atlasCoords);
}
