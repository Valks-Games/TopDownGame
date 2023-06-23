namespace RTS;

public class TileData
{
    public Vector2I TilePosition { get; }
    public float Weight { get; }

    public TileData(Vector2I tilePosition, float weight)
    {
        this.TilePosition = tilePosition;
        this.Weight = weight;
    }
}
