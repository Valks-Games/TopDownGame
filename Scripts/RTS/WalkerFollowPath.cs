namespace RTS;

public partial class Walker
{
    AStarPathing pathingAlgorithm = new(true);
    List<Vector2> path = new List<Vector2>();
    bool hasRun = false;
    bool isRunning = false;
    int currentPathIndex = 0;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        FollowPath();
    }

    public void AccelerateToVelocity(Vector2 direction)
    {
        double x = Velocity.X + direction.X * Acceleration * GetProcessDeltaTime();
        double y = Velocity.Y + direction.Y * Acceleration * GetProcessDeltaTime();

        if (direction.X == 0)
            x = Mathf.Lerp(0, x, Mathf.Pow(2, -50 * GetProcessDeltaTime()));

        if (direction.Y == 0)
            y = Mathf.Lerp(0, y, Mathf.Pow(2, -50 * GetProcessDeltaTime()));

        x = Mathf.Clamp(x, -CalculatedMaxSpeed, CalculatedMaxSpeed);
        y = Mathf.Clamp(y, -CalculatedMaxSpeed, CalculatedMaxSpeed);

        Velocity = new Vector2((float)x, (float)y);
    }

    void GetPathing()
    {
        path = pathingAlgorithm.TriggerWalkToPoint(
            startCoord: GetGlobalPositionAsCoord(),
            endCoord: (Vector2I)(player.Position / World.TileSize));

        currentPathIndex = 0;
    }

    void FollowPath()
    {
        if (Debug)
            for (int i = 0; i < path.Count - 1; i++)
                debuglines.Add(new DebugLine(
                    ToLocal(path[i] * World.TileSize),
                    ToLocal(path[i + 1] * World.TileSize),
                    Colors.Orange,
                    2f));

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

            if (((Vector2)charPos).DistanceTo(path[currentPathIndex]) < 1)
                currentPathIndex++;
        }
    }

    void Decelerate() => AccelerateToVelocity(Vector2.Zero);
    Vector2I GetGlobalPositionAsCoord() => ((Vector2I)(GlobalPosition / World.TileSize));
}
