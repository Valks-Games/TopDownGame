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
            var touchTiles = CalculateTileVectors();
            var hasCollision =  ValidateCollisionForList(touchTiles);
            if(hasCollision) {
                SwitchState(Idle());
                return;
            }
            MoveCharacter(jumpPos);
            ScaleUpAndDown();
        };

        return state;
    }


#region Animation
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
        tweenScale.Animate("scale", JumpSizeScale, JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        tweenScale.Animate("scale", Vector2.One, JumpDuration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
    }
#endregion Animation
}
