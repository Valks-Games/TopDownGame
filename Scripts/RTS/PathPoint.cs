namespace RTS;

/// <summary>
/// A point for pathfinding, starting at the end of the path and indicating the previous Pathpoint until the start of the path.  
/// Keep using previousPoint until it is null to get the full path
/// </summary>
public class PathPoint{
    public Vector2I point { get; set; }
    public PathPoint previousPoint { get; set; }
    public int distance { get; set; }
}