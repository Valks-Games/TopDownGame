namespace RTS;

public class AtlasWeight
{
    public Vector2I TilePosition;
    public float Weight;

    public AtlasWeight(Vector2I tilePosition, float weight)
    {
        this.TilePosition = tilePosition;
        this.Weight = weight;
    }
}
