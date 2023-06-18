namespace RTS;

public partial class Slime
{
    State Slide()
    {
        var state = new State("Slide");

        state.Enter = async () =>
        {
            sprite.Play("idle");

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

            var tween = new GTween(this);
            tween.Create();
            tween.Animate("position",
                finalValue: slidePos,
                duration: 0.75)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);
            tween.Callback(() => SwitchState(Idle()));
        };

        return state;
    }
}
