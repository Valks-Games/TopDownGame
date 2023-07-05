namespace RTS;

public class Atlas
{
    public TileMap TileMap { get; }
    public FastNoiseLite FNL { get; }
    public Dictionary<string, AtlasWeight> TileData { get; }

    public Atlas(TileMap tileMap, FastNoiseLite fnl, Dictionary<string, AtlasWeight> tileData, float emptyWeight = 0f)
    {
        tileData.Add("empty", new AtlasWeight(Vector2I.Zero, emptyWeight));
        ValidateTileDataWeights(tileData);

        this.TileMap = tileMap;
        this.FNL = fnl;
        this.TileData = TransformWeightsToRange(tileData);
    }

    void ValidateTileDataWeights(Dictionary<string, AtlasWeight> tileData) 
    {
        // Pre transform validation => if weights are less than 0, throw an exception
        foreach (var pair in tileData)
            if (pair.Value.Weight < 0f )
                throw new Exception($"Weight cannot be less than 0, error thrown by {pair.Key}, weight: {pair.Value.Weight}");
    }

    public Dictionary<string, AtlasWeight> TransformWeightsToRange(Dictionary<string, AtlasWeight> dictionary)
    {
        Dictionary<string, AtlasWeight> result = new();

        float totalWeight = 0f;
        foreach (var pair in dictionary) totalWeight += pair.Value.Weight;
    
        // Set current value to the lowerbound of the range
        float currentValue = -1;

        // Transform the weights to be between [-1, 1], as per FastNoiseLite range
        foreach (var pair in dictionary)
        {
            AtlasWeight atlasWeight = pair.Value;
            float weight = atlasWeight.Weight;
    
            currentValue += weight / totalWeight * 2;
    
            result.Add(pair.Key, new AtlasWeight(atlasWeight.TilePosition, currentValue));
        }
        return result;
    }
}
