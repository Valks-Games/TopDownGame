namespace RTS;

public partial class Slime
{
    
    [Export] public double JumpDuration { get; set; } = 0.75d;
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

            MoveCharacter(jumpPos);
            ScaleUpAndDown();
        };

        return state;
    }

    private void MoveCharacter(Vector2 jumpPos)
    {
        var tweenPos = new GTween(this);
        tweenPos.Create();
        tweenPos.Animate("position", Position + jumpPos, JumpDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        tweenPos.Callback(() => SwitchState(Idle()));
    }

    private void ScaleUpAndDown()
    {
        var tweenScale = new GTween(sprite);
        tweenScale.Create();
        tweenScale.Animate("scale", new Vector2(3, 4), JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        tweenScale.Animate("scale", Vector2.One, JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
    }
}
