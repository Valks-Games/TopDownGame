namespace RTS;

public partial class Slime : Entity
{
    [Export] public Player Player { get; set; }

    protected override State InitialState() => Idle();
}
