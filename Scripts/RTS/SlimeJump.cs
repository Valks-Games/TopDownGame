namespace RTS;

public partial class Slime
{
    State Jump()
    {
        var state = new State("Jump");

        state.Enter = () =>
        {
            sprite.Play("idle");

            // Jump towards player
            var diff = player.Position - Position;
            var dir = diff.Normalized();

            // Do not go past player
            var dist = Mathf.Min(diff.Length(), MaxJumpDist);

            var jumpPos = dir * dist;

            var duration = 0.75d;

            var tweenPos = new GTween(this);
            tweenPos.Create();
            tweenPos.Animate("position", Position + jumpPos, duration)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
            tweenPos.Callback(() => SwitchState(Idle()));

            var tweenScale = new GTween(sprite);
            tweenScale.Create();
            tweenScale.Animate("scale", new Vector2(3, 4), duration / 2)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
            tweenScale.Animate("scale", Vector2.One, duration / 2)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
        };

        return state;
    }
}
