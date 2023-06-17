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
                    var foundPath = false;

                    while (!foundPath)
                    {
                        void CalculateSlidePos() =>
                            slidePos = Position + GUtils.RandDir(GD.RandRange(10, 40))
                                - sprite.GetSize();

                        CalculateSlidePos();
                        var raycast = new RayCast2D
                        {
                            TargetPosition = slidePos - Position
                        };
                        raycast.AddException(this);
                        AddChild(raycast);

                        await GUtils.WaitOneFrame(this);

                        if (raycast.IsColliding())
                        {
                            if (raycast.GetCollider() is TileMap tileMap)
                                if (tileMap.Name == "Trees")
                                    CalculateSlidePos();
                        }
                        else
                        {
                            foundPath = true;
                        }

                        raycast.QueueFree();
                    }
                    
                    SwitchState(Slide(slidePos));
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
