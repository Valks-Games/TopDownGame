namespace RTS;

public partial class Walker {
        [Export]
        public bool HasDiagonalMovement { get; set; } = true;
        private AStar2D AStar = new AStar2D();
        List<Vector2> path = new List<Vector2>();
        int currentPathIndex = 0;
        private Vector2 startPoint;
        private Vector2 endPoint;
        bool hasRun = false;
        bool isRunning = false;


        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            FollowPath();
        }

        public void ResetPaths(){
            
            startPoint = World.Instance.Trees.GetUsedRect().Position;
            endPoint = World.Instance.Trees.GetUsedRect().End;
        }

    // TODO: Bind this to a skill first
        public void TriggerWalkToPoint(Vector2I Tilecoord)
        {
            if(hasRun == false){
                if(HasDiagonalMovement) AddDiagonalMovement();
                ResetPaths();
                AStarConnectWalkablePoints(AStarAddWalkablePoint());
                hasRun = true;
            }
            if (AStar.GetPointCount() <= 0) AStarConnectWalkablePoints(AStarAddWalkablePoint());
            if (AStar.GetPointCount() > 0 && AStar.HasPoint(GetPointIndex(Tilecoord)) == false) return;
            if (AStar.GetPointCount() > 0 && AStar.HasPoint(GetPointIndex(GetGlobalPositionAsCoord())) == false) DestuckCharacter();

            path = GetAStarPath(Tilecoord);
            currentPathIndex = 0;
        }

        private void DestuckCharacter()
        {
            if(OS.IsDebugBuild() && Debug) GD.PushWarning("Character is stuck, teleporting to closest point");
            // find the closest point to the character and add it to the AStar
            var charPos = GetGlobalPositionAsCoord();
            var closestPoint = new Vector2();
            var closestDistance = 9999999f;
            
            foreach (var pointIndex in AStar.GetPointIds())
            {
                var point = AStar.GetPointPosition((int) pointIndex);
                var distance = ((Vector2)(charPos)).DistanceTo(point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            GlobalPosition = closestPoint * World.TileSize;
        }

        private void FollowPath()
        {
            if (Debug)  for (int i = 0; i < path.Count - 1; i++)
                debuglines.Add(new DebugLine(ToLocal(path[i] * World.TileSize), ToLocal(path[i + 1] * World.TileSize), Colors.Orange, 2f));

            if (currentPathIndex == path.Count)
            {
                Decelerate();
                SwitchState(Idle());
                isRunning = false;
                return;
            }
            if (currentPathIndex < path.Count)
            {
                var charPos = GetGlobalPositionAsCoord();
                var direction = path[currentPathIndex] - charPos;
                AccelerateToVelocity(direction);
                if (((Vector2) charPos).DistanceTo(path[currentPathIndex]) < 1)
                {
                    currentPathIndex++;
                }
            }
        }

        public void AccelerateToVelocity(Vector2 direction){
            double x = Velocity.X + direction.X * Acceleration * GetProcessDeltaTime();
            double y = Velocity.Y + direction.Y * Acceleration * GetProcessDeltaTime();

            if(direction.X == 0)
                x = Mathf.Lerp(0, x, Mathf.Pow(2, -50 * GetProcessDeltaTime()));
            
            if(direction.Y == 0)
                y = Mathf.Lerp(0, y, Mathf.Pow(2, -50 * GetProcessDeltaTime()));

            x = Mathf.Clamp(x, -CalculatedMaxSpeed, CalculatedMaxSpeed);
            y = Mathf.Clamp(y, -CalculatedMaxSpeed, CalculatedMaxSpeed);

            Velocity = new Vector2((float) x, (float) y);
        }

        private void Decelerate() => AccelerateToVelocity(Vector2.Zero);


        private List<Vector2> AStarAddWalkablePoint()
        {
            List<Vector2> points = new List<Vector2>();
            for (int y = 0; y < (int)endPoint.Y; y++)
            {
                for (int x = 0; x < (int)endPoint.X; x++)
                {
                    var wcol = World.Instance.Trees.GetCellTileData(0, new Vector2I(x, y));
                    if (wcol == null)
                    {
                        Vector2 result = new Vector2(x, y);
                        points.Add(result);
                        int pointIndex = GetPointIndex(x, y);
                        AStar.AddPoint(pointIndex, result);
                    }
                }
            }
            return points;
        }


        private int GetPointIndex(Vector2I vect) => GetPointIndex(vect.X, vect.Y);
        private int GetPointIndex(Vector2 vect) => GetPointIndex((int)vect.X, (int)vect.Y);
        private int GetPointIndex(int x, int y) =>  x + y * (int)endPoint.X;
        

        private List<Vector2> MOVEABLEDIRECTIONS = new List<Vector2>(){
            Vector2.Down,
            Vector2.Up,
            Vector2.Left,
            Vector2.Right,
        };

        private void AddDiagonalMovement()
        {
            MOVEABLEDIRECTIONS.Add(new Vector2(-1, -1));
            MOVEABLEDIRECTIONS.Add(new Vector2(-1, 1));
            MOVEABLEDIRECTIONS.Add(new Vector2(1, -1));
            MOVEABLEDIRECTIONS.Add(new Vector2(1, 1));
        }

        private void AStarConnectWalkablePoints(List<Vector2> points)
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
                        AStar.ConnectPoints(GetPointIndex(v), GetPointIndex(connectingPoint),true);
                    }
                }
            }
        }

        private List<Vector2> GetAStarPath(Vector2 attackTilemapPoint)
        {
            if (AStar.GetPointCount() <= 0) AStarConnectWalkablePoints(AStarAddWalkablePoint());
            Vector2 startCoord = GetGlobalPositionAsCoord();
            Vector2 endCoord = attackTilemapPoint;
            int startID = GetPointIndex(startCoord);
            int endID = GetPointIndex(endCoord);
            return new List<Vector2>(AStar.GetPointPath(startID, endID));
        }

        private Vector2I GetGlobalPositionAsCoord() => ((Vector2I) (GlobalPosition / World.TileSize));
    }