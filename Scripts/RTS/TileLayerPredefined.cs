namespace RTS;

public class TileLayerPredefined
{
    public Dictionary<Vector2I, Vector2I> Data { get; } = new();
    public Texture2D Image { get; set; }
    public int ZIndex { get; set; }
}
