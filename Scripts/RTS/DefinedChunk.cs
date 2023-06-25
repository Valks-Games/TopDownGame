namespace RTS;

public partial class DefinedChunk : Node2D
{
    public List<TileLayerPredefined> TileLayers { get; } = new();

    public void Init()
    {
        foreach (TileMap tileMap in GetChildren())
        {
            var layer0 = 0;
            var sourceId = 0;
            var tileAtlas = tileMap.TileSet.GetSource(sourceId) as TileSetAtlasSource;
            var tileLayer = new TileLayerPredefined
            {
                Image = tileAtlas.Texture,
                ZIndex = tileMap.ZIndex
            };

            foreach (var pos in tileMap.GetUsedCells(layer0))
            {
                var uvs = tileMap.GetCellAtlasCoords(layer0, pos);

                var chunkX = (int)Mathf.Floor(pos.X / World.ChunkSize);
                var chunkY = (int)Mathf.Floor(pos.Y / World.ChunkSize);

                //GD.Print($"({chunkX}, {chunkY})");

                tileLayer.Data[new Vector2I(chunkX, chunkY)][pos] = uvs;
            }

            TileLayers.Add(tileLayer);
        }
    }
}
