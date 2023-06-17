namespace RTS;

public partial class Slime
{
    GTimer timerPreJump;

    State PreJump()
    {
        var state = new State("Pre Jump");

        state.Enter = () =>
        {
            sprite.Play("pre_jump");
            timerPreJump = new GTimer(this, 1000);
            timerPreJump.Finished += () => SwitchState(Jump());
            timerPreJump.Start();
        };

        state.Update = () =>
        {
            var str = 0.3;
            var randX = (float)GD.RandRange(-str, str);
            var randY = (float)GD.RandRange(-str, str);
            sprite.Offset = new Vector2(randX, randY);
        };

        state.Exit = () =>
        {
            sprite.Offset = Vector2.Zero;
        };

        return state;
    }
}
