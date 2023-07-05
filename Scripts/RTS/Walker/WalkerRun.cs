namespace RTS;

public partial class Walker
{
    State Run()
    {
        var state = new State("Run");

        state.Enter = () =>
        {
            if (player == null)
            {
                SwitchState(Idle());
                return;
            }

            if (!isRunning)
                GetPathing();

            isRunning = true;
        };

        return state;
    }
}
