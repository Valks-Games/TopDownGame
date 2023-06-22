namespace RTS;

public class AtlasWeight
{
    public Vector2I TilePosition { get; }
    public float Weight { get; }

    public AtlasWeight(Vector2I tilePosition, float weight)
    {
        this.TilePosition = tilePosition;
        this.Weight = weight;
    }
}
