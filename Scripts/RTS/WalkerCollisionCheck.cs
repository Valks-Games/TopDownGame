namespace RTS;

public partial class Walker
{
    #region CollisionDetection
    /// <summary>
    /// Calculates all the tiles the character will touch on the tilemap coordinate system using the point it's moving towards
    /// </summary>
    /// <returns>A list of all unique tile coordinates which the character touches</returns>
    List<Vector2I> CalculateTileVectors()
    {
        // a list of unique tile vectors which either the corner or center of the character will touch on the tilemap coordinate system
        var movementPosition = player.Position;
        var tileVectors = new List<Vector2I>();
        var direction = Position - movementPosition;
        var validationPoints = GetOuterCornersAndCenter(direction);

        foreach (var tilePoint in validationPoints)
        {
            if (Debug)
                DebugLinesSetup(movementPosition, tilePoint);
            var charLinePositionOnTilemap = FloorToVector2I((Position + tilePoint) / World.TileSize);
            var movementPositionOnTilemap = FloorToVector2I((movementPosition + tilePoint)/ World.TileSize);
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
        if (Debug)
            QueueRedraw();
        return tileVectors;
    }


    /// <summary>
    /// Rounds the Vector down instead of rounding it towards zero
    /// </summary>
    Vector2I FloorToVector2I(Vector2 movementPos) =>
        new Vector2I((int)Math.Floor(movementPos.X), (int)Math.Floor(movementPos.Y));

    /// <summary>
    /// Returns the outer corners minus 1px and centerpoint of the tile based on the movement direction
    /// without these points, we'd only take the center of the tile into account
    /// but it's not because the center of the tile can pass the collision, that the corners can
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    List<Vector2I> GetOuterCornersAndCenter(Vector2 direction)
    {
        var corner = World.TileSize / 2 - 2;
        var result = new List<Vector2I>();
        result.Add(Vector2I.Zero);
        if ((direction.X >= 0 && direction.Y >= 0) || (direction.X < 0 && direction.Y < 0))
        {
            result.Add(new Vector2I(-corner, corner));
            result.Add(new Vector2I(corner, -corner));
        }
        else
        {
            result.Add(new Vector2I(-corner, -corner));
            result.Add(new Vector2I(corner, corner));
        }
        return result;
    }

    /// <summary>
    /// Checks the tilemap for collision polygons, returns true if there are any.
    /// </summary>
    /// <param name="movementPosition"></param>
    /// <returns></returns>
    bool ValidateTileVectorHasCollision(Vector2I tileVector)
    {

        var tile = World.Instance.Trees.GetCellTileData(0, tileVector);
        return tile == null ? false : tile.GetCollisionPolygonsCount(0) > 0;
    }

    /// <summary>
    /// Validate if any of the tiles withing the list has a collision
    /// </summary>
    /// <param name="tilesTouched">Each tile the character will touch</param>
    /// <returns></returns>
    bool ValidateCollisionForList(List<Vector2I> tilesTouched)
    {
        bool hasCollision = false;
        foreach (var tileVector in tilesTouched)
        {
            hasCollision = ValidateTileVectorHasCollision(tileVector);
            if (this.Debug)
                if (hasCollision)
                    Logger.LogWarning($"TileVector: {tileVector.X}, {tileVector.Y}, hascollision");
                else
                    Logger.Log($"TileVector: {tileVector.X}, {tileVector.Y}");
            if (hasCollision)
                break;
        }

        return hasCollision;
    }
    #endregion CollisionDetection
}
