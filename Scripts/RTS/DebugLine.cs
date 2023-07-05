namespace RTS;

public class DebugLine
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    public Color Color { get; set; }
    public float Width { get; set; }

    public DebugLine(Vector2 start, Vector2 end, Color color, float width)
    {
        this.Start = start;
        this.End = end;
        this.Color = color;
        this.Width = width;
    }
}
