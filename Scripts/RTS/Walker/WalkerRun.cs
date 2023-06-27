namespace RTS;

public partial class Walker
{
    
    State Run()
    {
        var state = new State("Jump");

        state.Enter = () =>
        {
            if(player == null) {
                SwitchState(Idle());
                return;
            }

            var jumpPos = CalculateJumpPosition();
            var touchTiles = CalculateTileVectors();
            var hasCollision =  ValidateCollisionForList(touchTiles);
            if(hasCollision) {
                CalculateMovableCoordinates();
                var pathToPlayer = GetPathPoint((Vector2I)(player.Position / World.TileSize));
                // should be run to enemy
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
