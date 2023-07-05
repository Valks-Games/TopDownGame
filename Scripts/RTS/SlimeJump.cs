namespace RTS;

public partial class Slime
{
    State Jump()
    {
        var state = new State("Jump");

        state.Enter = () =>
        {
            sprite.Play("idle");

            var jumpPos = CalculateJumpPosition();
            MoveCharacter(jumpPos);
            ScaleUpAndDown();
        };

        return state;
    }

    Vector2 CalculateJumpPosition()
    {
        // Jump towards player
        var diff = player.Position - Position;
        var dir = diff.Normalized();

        // Do not go past player
        var dist = Mathf.Min(diff.Length(), MaxJumpDist);

        return dir * dist;
    }

    #region Animation
    void MoveCharacter(Vector2 jumpPos)
    {
        var tweenPos = new GTween(this);
        tweenPos.Create();
        tweenPos.Animate("position", Position + jumpPos, JumpDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        tweenPos.Callback(() => SwitchState(Idle()));
    }

    void ScaleUpAndDown()
    {
        var tweenScale = new GTween(sprite);
        tweenScale.Create();
        tweenScale.Animate("scale", JumpSizeScale, JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        tweenScale.Animate("scale", Vector2.One, JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
    }
    #endregion Animation
}
