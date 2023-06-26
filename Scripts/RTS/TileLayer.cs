namespace RTS;

public class TileLayer
{
    public int ZIndex { get; }
    public FastNoiseLite FNL { get; }
    public Texture2D TileSetImage { get; }
    public Dictionary<string, TileData> TileData { get; }

    public TileLayer(int zindex, string tileSetImagePath, FastNoiseLite fnl, Dictionary<string, TileData> tileData, float emptyWeight = 0f)
    {
        tileData.Add("empty", new TileData(Vector2I.Zero, emptyWeight));
        ValidateTileDataWeights(tileData);

        this.TileSetImage = GD.Load<Texture2D>($"res://{tileSetImagePath}");
        this.ZIndex = zindex;
        this.FNL = fnl;
        this.TileData = TransformWeightsToRange(tileData);
    }

    void ValidateTileDataWeights(Dictionary<string, TileData> tileData) 
    {
        // Pre transform validation => if weights are less than 0, throw an exception
        foreach (var pair in tileData)
            if (pair.Value.Weight < 0f )
                throw new Exception($"Weight cannot be less than 0, error thrown by {pair.Key}, weight: {pair.Value.Weight}");
    }

    public Dictionary<string, TileData> TransformWeightsToRange(Dictionary<string, TileData> dictionary)
    {
        Dictionary<string, TileData> result = new();

        float totalWeight = 0f;
        foreach (var pair in dictionary) totalWeight += pair.Value.Weight;
    
        // Set current value to the lowerbound of the range
        float currentValue = -1;

        // Transform the weights to be between [-1, 1], as per FastNoiseLite range
        foreach (var pair in dictionary)
        {
            TileData atlasWeight = pair.Value;
            float weight = atlasWeight.Weight;
    
            currentValue += weight / totalWeight * 2;
    
            result.Add(pair.Key, new TileData(atlasWeight.UV, currentValue, atlasWeight.Collision));
        }
        return result;
    }
}
