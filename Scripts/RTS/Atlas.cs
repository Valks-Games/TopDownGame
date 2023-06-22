namespace RTS;

public class Atlas
{
    public TileMap TileMap { get; }
    public FastNoiseLite FNL { get; }
    public Dictionary<string, AtlasWeight> TileData { get; }

    public Atlas(TileMap tileMap, FastNoiseLite fnl, Dictionary<string, AtlasWeight> tileData)
    {
        this.TileMap = tileMap;
        this.FNL = fnl;
        this.TileData = tileData;
        this.TileData.Add("empty", new AtlasWeight(Vector2I.Zero, 0));
    }
}
