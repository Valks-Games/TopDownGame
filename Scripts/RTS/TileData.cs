namespace RTS;

public class TileData
{
    public Vector2I UV { get; }
    public float Weight { get; }
    public bool Collision { get; }

    public TileData(Vector2I uv, float weight, bool collision = false)
    {
        this.UV = uv;
        this.Weight = weight;
        this.Collision = collision;
    }
}
