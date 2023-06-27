namespace RTS;

public partial class Walker
{
    
    State Run()
    {
        var state = new State("Run");

        state.Enter = () =>
        {
            if(player == null) {
                SwitchState(Idle());
                return;
            }

            if(!isRunning) TriggerWalkToPoint((Vector2I)(player.Position / World.TileSize));
            isRunning = true;
        };

        return state;
    }

}