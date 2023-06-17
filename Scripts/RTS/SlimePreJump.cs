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
            timerPreJump = new GTimer(this, DurationPreJump);
            timerPreJump.Finished += () => 
            {
                if (player != null)
                {
                    // Jump towards player
                    var diff = player.Position - Position;
                    var dir = diff.Normalized();

                    // Do not go past player
                    var dist = Mathf.Min(diff.Length(), MaxJumpDist);

                    var jumpPos = dir * dist;

                    SwitchState(Jump(jumpPos));
                }
                else
                {
                    // Slide in random direction
                    SwitchState(Slide());
                }
            };
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
