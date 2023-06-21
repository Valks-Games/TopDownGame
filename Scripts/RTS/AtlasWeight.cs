namespace RTS;
using Godot;

public class AtlasWeight
{
    public AtlasWeight(Vector2I tilePosition, float weight)
    {
        this.TilePosition = tilePosition;
        this.Weight = weight;
    }
    public Vector2I TilePosition;
    public float Weight;
}