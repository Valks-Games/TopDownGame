namespace RTS;

public partial class Slime
{
    State Idle()
    {
        var state = new State("Idle");

        state.Enter = () =>
        {
            sprite.Play("idle");

            GetTree().CreateTimer(DurationIdle).Timeout += () =>
                SwitchState(PreJump());
        };

        return state;
    }
}
