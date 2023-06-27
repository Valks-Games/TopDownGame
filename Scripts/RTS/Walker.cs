namespace RTS;

public partial class Walker : Monster
{
    [Export] public int DurationIdle { get; set; } = 2000;
    [Export] public bool Debug { get; set; } = false;
    [Export] public int MaxPathingDistanceInTiles { get; set; } = 20;
    [Export] public bool CanFly { get; set; } = false;
    
    [Export]
    public float MaxSpeed { get; set; } = 1000;
    [Export]
    public float Acceleration { get; set; } = 10000;
    
    public float ModifierMultiplier { get; set; } = 1;
    public float CalculatedMaxSpeed => MaxSpeed * ModifierMultiplier;
    

    protected override State InitialState() => Idle();

    #region Debugging
    List<DebugLine> debuglines = new List<DebugLine>();
    private class DebugLine
    {
        public Vector2 start;
        public Vector2 end;
        public Color color;
        public float width;
        public DebugLine(Vector2 start, Vector2 end, Color color, float width)
        {
            this.start = start;
            this.end = end;
            this.color = color;
            this.width = width;
        }
    }
    
    private void DebugLinesSetup(Vector2 movementPosition, Vector2I tilePoint)
    {
        var localCharLinePos = ToLocal(Position + tilePoint);
        var localMovementPos = ToLocal(movementPosition + tilePoint);
        debuglines.Add(new DebugLine(localCharLinePos, localMovementPos, Colors.Orange, 2f));
    }

    public override void _Process(double delta)
    {
        if(OS.IsDebugBuild() == true || Debug == true) QueueRedraw();
        base._Process(delta);
    }
    
    public override void _Draw(){

        if(OS.IsDebugBuild() == false || Debug == false) return;
        
        foreach (var line in debuglines)
        {
            DrawLine(line.start, line.end, line.color, line.width);
        }
        debuglines.Clear();
    }
#endregion Debugging
}
