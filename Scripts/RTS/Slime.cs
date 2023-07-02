namespace RTS;

public partial class Slime : Monster
{
    [Export] public int MaxJumpDist { get; set; } = 100;
    [Export] public float DurationIdle { get; set; } = 2;
    [Export] public float DurationPreJump { get; set; } = 1;

    protected override State InitialState() => Idle();
}
