namespace RTS;

public class AtlasWeight
{
    public Vector2I TilePosition { get; }
    public float Weight { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <param name="weight"> 
    /// The weight of the tile must be 0 or greater.  A weight of 0 means this tile will not be used, 
    /// even though the tile is added to the atlas.
    /// 
    /// - If multiple tiles are added with the same weight, their frequency of use will be equal.
    /// - If multiple tiles are added with different weights, their frequency of use will be proportional to their weight, compared to the total weight of all the tiles within the atlas.
    /// </param>
    public AtlasWeight(Vector2I tilePosition, float weight)
    {
        this.TilePosition = tilePosition;
        this.Weight = weight;
    }
}
