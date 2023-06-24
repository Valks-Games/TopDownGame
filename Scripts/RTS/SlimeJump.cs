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
                CalculateMovableCoordinates();
                var points = pathPoints;
                SwitchState(Idle());
                return;
            }
            MoveCharacter(jumpPos);
            ScaleUpAndDown();
        };

        return state;
    }
    private Vector2 CalculateJumpPosition(){
        // Jump towards player
        var diff = player.Position - Position;
        var dir = diff.Normalized();

        // Do not go past player
        var dist = Mathf.Min(diff.Length(), MaxJumpDist);

        return dir * dist;
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
