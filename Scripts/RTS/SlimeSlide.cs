namespace RTS;

public partial class Slime
{
    State Slide(Vector2 slidePos)
    {
        var state = new State("Slide");

        state.Enter = () =>
        {
            sprite.Play("idle");

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
