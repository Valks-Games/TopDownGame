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
            timerPreJump.Finished += async () => 
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
                    Vector2 slidePos = Vector2.Zero;

                    Vector2 CalculateSlidePos()
                    {
                        var dir = GUtils.RandDir();
                        var dist = GD.RandRange(10, 40);

                        slidePos = Position + (dist * dir);

                        return dir;
                    }

                    var dir = CalculateSlidePos();
                    var raycast = new RayCast2D
                    {
                        TargetPosition = slidePos - Position
                    };
                    raycast.AddException(this);

                    AddChild(raycast);

                    // Required for the raycast to do its job
                    await GUtils.WaitOneFrame(this);

                    if (raycast.IsColliding())
                    {
                        slidePos = raycast.GetCollisionPoint() -
                            (dir * (sprite.GetSize() / 2));
                    }

                    raycast.QueueFree();

                    SwitchState(Slide(slidePos));
                }
            };
            timerPreJump.Start();
        };

        state.Update = () =>
        {
            var str = player == null ? 0.05 : 0.4;
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
