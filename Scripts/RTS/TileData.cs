namespace RTS;

public class TileData
{
    public Vector2I Atlas { get; }
    public float Weight { get; }
    public bool Collision { get; }

    public TileData(Vector2I atlas, float weight, bool collision = false)
    {
        this.Atlas = atlas;
        this.Weight = weight;
        this.Collision = collision;
    }
}
