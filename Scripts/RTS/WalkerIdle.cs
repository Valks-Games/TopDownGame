namespace RTS;

public partial class Walker
{
    GTimer timerIdle;

    State Idle()
    {
        var state = new State("Idle");

        state.Enter = () =>
        {
            sprite.Play("idle");
            timerIdle = new GTimer(this, DurationIdle);
            timerIdle.Finished += () => SwitchState(Run());
            timerIdle.Start();
        };

        return state;
    }
}
