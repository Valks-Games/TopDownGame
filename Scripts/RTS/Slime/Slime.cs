using Template;

namespace RTS;

public partial class Slime : Monster
{
    [Export] public int MaxJumpDist { get; set; } = 100;
    [Export] public int DurationIdle { get; set; } = 2000;
    [Export] public int DurationPreJump { get; set; } = 1000;
    [Export] public double JumpDuration { get; set; } = 0.75d;
    [Export] public Vector2 JumpSizeScale { get; set; } = new Vector2(3, 4);
    [Export] public bool Debug { get; set; } = false;
    [Export] public int MaxPathingDistanceInTiles { get; set; } = 20;
    // TODO: Should be moved to Monster.cs?
    [Export] public bool CanFly { get; set; } = false;

    protected override State InitialState() => Idle();

    #region Debugging
    List<DebugLine> lines = new List<DebugLine>();

    public override void _Draw()
    {
        foreach (var line in lines)
            DrawLine(line.Start, line.End, line.Color, line.Width);

        lines.Clear();
    }

    void DebugLinesSetup(Vector2 movementPosition, Vector2I tilePoint)
    {
        var localCharLinePos = ToLocal(Position + tilePoint);
        var localMovementPos = ToLocal(movementPosition + tilePoint);

        lines.Add(new DebugLine(
            start: localCharLinePos, 
            end: localMovementPos, 
            color: Colors.Orange, 
            width: 2));
    }
    #endregion Debugging
}
