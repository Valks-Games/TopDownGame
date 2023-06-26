namespace RTS;

public partial class Slime : Monster
{
    [Export] public int MaxJumpDist { get; set; } = 100;
    [Export] public int DurationIdle { get; set; } = 2000;
    [Export] public int DurationPreJump { get; set; } = 1000;

    protected override State InitialState() => Idle();
}
