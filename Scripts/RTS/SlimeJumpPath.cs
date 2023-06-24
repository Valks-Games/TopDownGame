namespace RTS;

public partial class Slime {
    private int remainingSpeed = 0;
    // calculation Lists
    private List<Vector2I> currentSpeedCoordinates;
    private List<Vector2I> nextSpeedCoordinates;
    private List<Vector2I> collectedCoordinates;
    // calculated Path
    public Dictionary<Vector2I, PathPoint> pathPoints { get; private set; }

    public List<Vector2I> CalculateMovableCoordinates()
    {
        ResetLists();
        GetCoordinates();
        return collectedCoordinates;
    }

    /// <summary>
    /// Reset all lists, dictionaries and values so we can start a new calculation
    /// </summary>
    private void ResetLists()
    {
        nextSpeedCoordinates = new List<Vector2I>();
        collectedCoordinates = new List<Vector2I>(); 
        pathPoints = new Dictionary<Vector2I, PathPoint>();

        AddVectorToLists((Vector2I) Position); // add current position to the list
        
        currentSpeedCoordinates = new List<Vector2I>(collectedCoordinates);
        remainingSpeed = MaxPathingDistanceInTiles;
    }

    /// <summary>
    /// Convert the new position to a Pathpoint
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="previousPoint"></param>
    /// <param name="dist"></param>
    private void AddVectorToLists(Vector2I coord, PathPoint previousPoint = null, int dist = 0){
        collectedCoordinates.Add(coord);
        PathPoint movePoint = new PathPoint(){
            point = coord,
            previousPoint = previousPoint,
            distance = dist
        };
        pathPoints.Add(movePoint.point, movePoint);
    }

    /// <summary>
    /// Goes through all coordinates and adds the next possible coordinates to the list
    /// If the next coordinate is already in the list, it will not be added again
    /// If the Players position has been found stop the loop, else keep looping until we run out of speed
    /// </summary>
    private void GetCoordinates()
    {
        while (remainingSpeed > 0)
        {
            foreach (Vector2I coord in currentSpeedCoordinates)
            {

                // We could make this a dynamic function, because now it's limited to one tile walking distance, but our char can walk further
                // in one step... We should calculate all potential Vector2I's the character could run then run the validation on all of them
                // for the current coordinate.
                // this could bloat our overhead though, we'll need less steps but more calculations per step
                if (ValidateNextCoordinate(coord - Vector2I.Left, coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - Vector2I.Right, coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - Vector2I.Up, coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - Vector2I.Down, coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - new Vector2I( 1, 1), coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - new Vector2I(-1, 1), coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - new Vector2I( 1,-1), coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
                if (ValidateNextCoordinate(coord - new Vector2I(-1,-1), coord, MaxPathingDistanceInTiles - remainingSpeed + 1)) return;
            }
            remainingSpeed--;
            currentSpeedCoordinates = new List<Vector2I>(nextSpeedCoordinates);
            nextSpeedCoordinates.Clear();
            
        }
    }

    /// <summary>
    /// Validate one unique coordinate and add it to the list if it is valid and not already in the list
    /// </summary>
    /// <param name="coord">The point we're moving to</param>
    /// <param name="previousCoord">The point we came from</param>
    /// <param name="distance">The distance we moved to get to this point</param>
    /// <returns>If we find the players position, we don't need to continue and we return "true"</returns>
    private bool ValidateNextCoordinate(Vector2I coord, Vector2I previousCoord, int distance)
    {
        var colTile = World.Instance.Trees.GetCellTileData(0, coord);

        if(colTile != null && !CanFly) 
            return false;

        if (!collectedCoordinates.Contains(coord))
        {
            nextSpeedCoordinates.Add(coord);
            PathPoint previousPoint = pathPoints[previousCoord];
            AddVectorToLists(coord,previousPoint, distance);
            return coord == (Vector2I) player.Position;
        }
        return false;
    }
}