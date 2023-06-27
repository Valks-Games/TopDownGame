namespace RTS;

public class TileLayer
{
    public TileMap TileMap { get; }
    public FastNoiseLite FNL { get; }
    public Dictionary<string, TileData> TileData { get; }

    public TileLayer(TileMap tileMap, FastNoiseLite fnl, Dictionary<string, TileData> tileData, float emptyWeight = 0f)
    {
        tileData.Add("empty", new TileData(Vector2I.Zero, emptyWeight));
        ValidateTileDataWeights(tileData);

        this.TileMap = tileMap;
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

        var totalWeight = 0f;

        foreach (var pair in dictionary) 
            totalWeight += pair.Value.Weight;
    
        // Set current value to the lowerbound of the range
        var currentValue = -1f;

        // Transform the weights to be between [-1, 1], as per FastNoiseLite range
        foreach (var pair in dictionary)
        {
            TileData atlasWeight = pair.Value;
            float weight = atlasWeight.Weight;
    
            currentValue += weight / totalWeight * 2;
    
            result.Add(pair.Key, new TileData(atlasWeight.Atlas, atlasWeight.Weight, atlasWeight.Collision));
        }
        return result;
    }
}
