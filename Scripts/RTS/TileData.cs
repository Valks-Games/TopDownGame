namespace RTS;

public class TileData
{
    public Vector2I UV { get; }
    public float Weight { get; }

    public TileData(Vector2I uv, float weight)
    {
        this.UV = uv;
        this.Weight = weight;
    }
}
