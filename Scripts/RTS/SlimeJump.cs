namespace RTS;

public partial class Slime
{
    State Jump()
    {
        var state = new State("Jump");

        state.Enter = () =>
        {
            sprite.Play("idle");

            var diff = Player.Position - Position;
            var dir = diff.Normalized();

            var maxJumpDist = 100;

            // Do not go past player
            var dist = Mathf.Min(diff.Length(), maxJumpDist);

            var jumpPos = dir * dist;
            var duration = 0.75d;

            var tweenPos = new GTween(this);
            tweenPos.Create();
            tweenPos.Animate("position", Position + jumpPos, duration)
                .SetTrans(Tween.TransitionType.Sine);
            tweenPos.Callback(() => SwitchState(Idle()));

            var tweenScale = new GTween(sprite);
            tweenScale.Create();
            tweenScale.Animate("scale", Vector2.One * 3, duration / 2)
                .SetTrans(Tween.TransitionType.Sine);
            tweenScale.Animate("scale", Vector2.One, duration / 2)
                .SetTrans(Tween.TransitionType.Sine);
        };

        return state;
    }
}
