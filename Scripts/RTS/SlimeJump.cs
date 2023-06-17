namespace RTS;

public partial class Slime
{
    State Jump(Vector2 jumpPos)
    {
        var state = new State("Jump");

        state.Enter = () =>
        {
            sprite.Play("idle");

            var duration = 0.75d;

            var tweenPos = new GTween(this);
            tweenPos.Create();
            tweenPos.Animate("position", Position + jumpPos, duration)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
            tweenPos.Callback(() => SwitchState(Idle()));

            var tweenScale = new GTween(sprite);
            tweenScale.Create();
            tweenScale.Animate("scale", Vector2.One * 3, duration / 2)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
            tweenScale.Animate("scale", Vector2.One, duration / 2)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
        };

        return state;
    }
}
