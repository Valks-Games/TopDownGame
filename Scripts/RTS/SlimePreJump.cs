namespace RTS;

public partial class Slime
{
    State PreJump()
    {
        var state = new State("Pre Jump");

        state.Enter = () =>
        {
            sprite.Play("pre_jump");

            var tween = new GTween(sprite);
            tween.Create();
            tween.Animate("scale", new Vector2(1.1f, 0.9f), DurationPreJump / 1000d);

            GetTree().CreateTimer(DurationPreJump).Timeout += () =>
            {
                if (player != null)
                {
                    SwitchState(Jump());
                }
                else
                {
                    SwitchState(Slide());
                }

            };
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
            sprite.Scale = Vector2.One;
        };

        return state;
    }
}
