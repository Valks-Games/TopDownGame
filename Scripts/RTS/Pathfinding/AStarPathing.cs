namespace RTS;

public partial class AStarPathing
{
    public bool Debug { get; set; } = false;
    
    AStar2D aStar = new();
    Vector2 startPoint;
    Vector2 endPoint;
    bool init = false;

    public AStarPathing(bool hasDiagonalMovement = true)
    {
        if (hasDiagonalMovement)
            AddDiagonalMovement();
    }

    public List<Vector2> TriggerWalkToPoint(Vector2I startCoord, Vector2I endCoord)
    {
        if (!init)
            Init();

        if (aStar.GetPointCount() <= 0)
            AStarConnectWalkablePoints(AStarAddWalkablePoint());

        if (aStar.GetPointCount() > 0 && 
            !aStar.HasPoint(GetPointIndex(endCoord)))
            return new List<Vector2>();

        //if (aStar.GetPointCount() > 0 && !aStar.HasPoint(GetPointIndex(startCoord))) DestuckCharacter(startCoord);

        return GetAStarPath(startCoord, endCoord);
    }

    //Destuck logic
    // this should actually not happen
    // private void DestuckCharacter(Vector2I charPos)
    // {
    //     if (OS.IsDebugBuild() && Debug)
    //         GD.PushWarning("Character is stuck, teleporting to closest point");
    //     // find the closest point to the character and add it to the AStar
    //     var closestPoint = new Vector2();
    //     var closestDistance = 9999999f;

    //     foreach (var pointIndex in aStar.GetPointIds())
    //     {
    //         var point = aStar.GetPointPosition((int) pointIndex);
    //         var distance = ((Vector2)(charPos)).DistanceTo(point);
    //         if (distance < closestDistance)
    //         {
    //             closestDistance = distance;
    //             closestPoint = point;
    //         }
    //     }

    // this couples the code to the Characterbody2d which is not good
    // decouple this if we need destuck
    //     owner.GlobalPosition = closestPoint * World.TileSize;
    // }

    void Init()
    {
        ResetPaths();
        AStarConnectWalkablePoints(AStarAddWalkablePoint());
        init = true;
    }

    void AddDiagonalMovement()
    {
        MOVEABLEDIRECTIONS.Add(new Vector2(-1, -1));
        MOVEABLEDIRECTIONS.Add(new Vector2(-1, 1));
        MOVEABLEDIRECTIONS.Add(new Vector2(1, -1));
        MOVEABLEDIRECTIONS.Add(new Vector2(1, 1));
    }

    void AStarConnectWalkablePoints(List<Vector2> points)
    {
        foreach (Vector2 v in points)
        {
            foreach (Vector2 direction in MOVEABLEDIRECTIONS)
            {
                Vector2 connectingPoint = v + direction;

                if (points.Contains(connectingPoint))
                {
                    var index = GetPointIndex(v);
                    var index2 = GetPointIndex(connectingPoint);

                    aStar.ConnectPoints(
                        id: GetPointIndex(v),
                        toId: GetPointIndex(connectingPoint), 
                        bidirectional: true);
                }
            }
        }
    }

    void ResetPaths()
    {
        // this is the startpoint and endpoint of the area that needs to have pathfinding
        // if we set this to the entire world, we should only have ONE instance of this class and just reuse it
        startPoint = World.Instance.Trees.GetUsedRect().Position;
        endPoint = World.Instance.Trees.GetUsedRect().End;
    }

    int GetPointIndex(Vector2I vect) => GetPointIndex(vect.X, vect.Y);
    int GetPointIndex(Vector2 vect) => GetPointIndex((int)vect.X, (int)vect.Y);
    int GetPointIndex(int x, int y) => x + y * (int)endPoint.X;

    List<Vector2> AStarAddWalkablePoint()
    {
        var points = new List<Vector2>();

        for (int y = 0; y < (int)endPoint.Y; y++)
        {
            for (int x = 0; x < (int)endPoint.X; x++)
            {
                var wcol = World.Instance.Trees.GetCellTileData(
                    layer: 0,
                    coords: new Vector2I(x, y));

                if (wcol == null)
                {
                    var result = new Vector2(x, y);
                    points.Add(result);

                    int pointIndex = GetPointIndex(x, y);
                    aStar.AddPoint(pointIndex, result);
                }
            }
        }

        return points;
    }

    List<Vector2> GetAStarPath(Vector2 startCoord, Vector2 endCoord)
    {
        int startID = GetPointIndex(startCoord);
        int endID = GetPointIndex(endCoord);

        return new List<Vector2>(aStar.GetPointPath(startID, endID));
    }

    List<Vector2> MOVEABLEDIRECTIONS = new List<Vector2>(){
        Vector2.Down,
        Vector2.Up,
        Vector2.Left,
        Vector2.Right,
    };
}
