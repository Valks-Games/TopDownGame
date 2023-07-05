namespace RTS;

public partial class Walker : Monster
{
    [Export] public int DurationIdle { get; set; } = 2000;
    [Export] public bool Debug { get; set; } = false;
    [Export] public int MaxPathingDistanceInTiles { get; set; } = 20;
    [Export] public bool CanFly { get; set; } = false;
    [Export] public float MaxSpeed { get; set; } = 1000;
    [Export] public float Acceleration { get; set; } = 10000;

    public float ModifierMultiplier { get; set; } = 1;
    public float CalculatedMaxSpeed => MaxSpeed * ModifierMultiplier;

    protected override State InitialState() => Idle();

    #region Debugging
    List<DebugLine> debuglines = new List<DebugLine>();

    public override void _Process(double delta)
    {
        if (OS.IsDebugBuild() || Debug)
            QueueRedraw();

        base._Process(delta);
    }

    public override void _Draw()
    {
        if (!OS.IsDebugBuild() || !Debug)
            return;

        foreach (var line in debuglines)
            DrawLine(line.Start, line.End, line.Color, line.Width);

        debuglines.Clear();
    }

    void DebugLinesSetup(Vector2 movementPosition, Vector2I tilePoint)
    {
        var localCharLinePos = ToLocal(Position + tilePoint);
        var localMovementPos = ToLocal(movementPosition + tilePoint);

        debuglines.Add(new DebugLine(
            start: localCharLinePos,
            end: localMovementPos,
            color: Colors.Orange,
            width: 2));
    }
    #endregion Debugging
}
