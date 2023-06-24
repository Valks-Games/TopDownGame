namespace RTS;

public class TileLayer
{
    public int ZIndex { get; }
    public FastNoiseLite FNL { get; }
    public Texture2D TileSetImage { get; }
    public List<TileData> TileData { get; }

    public TileLayer(int zindex, string tileSetImagePath, FastNoiseLite fnl, List<TileData> tileData, float emptyWeight = 0f)
    {
        tileData.Add(new TileData(Vector2I.Zero, emptyWeight));
        ValidateTileDataWeights(tileData);

        this.TileSetImage = GD.Load<Texture2D>($"res://{tileSetImagePath}");
        this.ZIndex = zindex;
        this.FNL = fnl;
        this.TileData = TransformWeightsToRange(tileData);
    }

    void ValidateTileDataWeights(List<TileData> tileData) 
    {
        // Pre transform validation => if weights are less than 0, throw an exception
        foreach (var tile in tileData)
            if (tile.Weight < 0f )
                throw new Exception($"Weight cannot be less than 0, error thrown " +
                    $"by {tile.Name}, weight: {tile.Weight}");
    }

    public List<TileData> TransformWeightsToRange(List<TileData> tileData)
    {
        List<TileData> result = new();

        var totalWeight = 0f;
        foreach (var tile in tileData) 
            totalWeight += tile.Weight;
    
        // Set current value to the lowerbound of the range
        var currentValue = -1f;

        // Transform the weights to be between [-1, 1], as per FastNoiseLite range
        foreach (var tile in tileData)
        {
            currentValue += tile.Weight / totalWeight * 2;
    
            result.Add(new TileData(tile.UV, currentValue, tile.Collision));
        }
        return result;
    }
}
