namespace RTS;


public class PathPoint{
    public Vector2I point { get; set; }
    public PathPoint previousPoint { get; set; }
    public int distance { get; set; }
}