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
#region Pathing
    private Vector2 CalculateJumpPosition(){
        // Jump towards player
        var diff = player.Position - Position;
        var dir = diff.Normalized();

        // Do not go past player
        var dist = Mathf.Min(diff.Length(), MaxJumpDist);

        return dir * dist;
    }


    /// <summary>
    /// Calculates all the tiles the character will touch on the tilemap coordinate system using the point it's moving towards
    /// </summary>
    /// <param name="movementPosition">Point the character is moving to in world coordinates, NOT tile coordinates</param>
    /// <returns></returns>
    private List<Vector2I> CalculateTileVectors() {
        // a list of unique tile vectors which either the corner or center of the character will touch on the tilemap coordinate system
        var tileVectors = new List<Vector2I>();
        // get the center of the tile the character is currently on, else the corner points will be on another tile which has a collider
        var centerTileMovementPosition = FloorToVector2I(player.Position) ;
        var centerTilePosition = FloorToVector2I(Position) ;
        var direction = (Vector2I) ((centerTilePosition - centerTileMovementPosition ) / World.TileSize);
        var validationPoints = GetOuterCornersAndCenter(direction);
        
        foreach (var tilePoint in validationPoints)
        {
            var charLinePositionOnTilemap = FloorToVector2I((centerTilePosition + tilePoint) / World.TileSize);
            var movementPositionOnTilemap = FloorToVector2I((centerTileMovementPosition + tilePoint)/ World.TileSize);
            var lineDirection = movementPositionOnTilemap - charLinePositionOnTilemap;


            // we need to validate which tile the character is on for each round number on the X and Y axis
            float xStep = lineDirection.Y == 0 ? 0 : (float)(lineDirection.X / lineDirection.Y);
            float yStep = lineDirection.X == 0 ? 0 : (float)(lineDirection.Y / lineDirection.X);

            int increment = lineDirection.X < 0 ? 1 : -1;
            for (int x = lineDirection.X; x != 0; x += increment)
            {
                var nextPosition = new Vector2I(charLinePositionOnTilemap.X + x, charLinePositionOnTilemap.Y + (int)(yStep * x));
                if (!tileVectors.Contains(nextPosition))
                    tileVectors.Add(nextPosition);
            }

            increment = lineDirection.Y < 0 ? 1 : -1;
            for (int y = lineDirection.Y; y != 0; y += increment)
            {
                var nextPosition = new Vector2I(charLinePositionOnTilemap.X + (int)(xStep * y), charLinePositionOnTilemap.Y + y);
                if (!tileVectors.Contains(nextPosition))
                    tileVectors.Add(nextPosition);
            }
        }
        return tileVectors;
    }

    private Vector2I FloorToVector2I(Vector2 movementPos) =>
        new Vector2I((int)Math.Floor(movementPos.X), (int)Math.Floor(movementPos.Y));

    /// <summary>
    /// Returns the outer corners and centerpoint of the tile based on the movement direction
    /// without these points, we'd only take the center of the tile into account
    /// but it's not because the center of the tile can pass the collision, that the corners can
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private List<Vector2I> GetOuterCornersAndCenter(Vector2 direction){
        var result = new List<Vector2I>();
        var corner = World.TileSize / 2 - 1;
        result.Add(Vector2I.Zero);
        if((direction.X >= 0 && direction.Y >= 0) || (direction.X < 0 && direction.Y < 0)) {
            result.Add(new Vector2I(-corner, corner));
            result.Add(new Vector2I( corner,-corner));
        } else {
            result.Add(new Vector2I(-corner,-corner));
            result.Add(new Vector2I( corner, corner));
        }
        return result;
    }

    /// <summary>
    /// Checks the tilemap for collision polygons, returns true if there are any.
    /// </summary>
    /// <param name="movementPosition"></param>
    /// <returns></returns>
    private bool ValidateTileVectorHasCollision(Vector2I tileVector) {
        
        var tile = World.Instance.Trees.GetCellTileData(0, tileVector);
        return tile == null ? false : tile.GetCollisionPolygonsCount(0) > 0;
    }

    /// <summary>
    /// Validate if any of the tiles withing the list has a collision
    /// </summary>
    /// <param name="tilesTouched">Each tile the character will touch</param>
    /// <returns></returns>
    private bool ValidateCollisionForList(List<Vector2I> tilesTouched)
    {
        bool hasCollision = false;
        foreach (var tileVector in tilesTouched)
        {
            hasCollision = ValidateTileVectorHasCollision(tileVector);
            if (hasCollision)
                break;
        }

        return hasCollision;
    }
#endregion Pathing
}
