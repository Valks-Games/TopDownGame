namespace RTS;

public class Atlas
{
    public int ZIndex { get; }
    public TileMap TileMap { get; }
    public FastNoiseLite FNL { get; }
    public Dictionary<string, AtlasWeight> TileData { get; }

    public Atlas(int zindex, TileMap tileMap, FastNoiseLite fnl, Dictionary<string, AtlasWeight> tileData, float emptyWeight = 0f)
    {
        tileData.Add("empty", new AtlasWeight(new Vector2I(0, 0), emptyWeight));

        this.ZIndex = zindex;
        this.TileMap = tileMap;
        this.FNL = fnl;
        this.TileData = TransformWeightsToRange(tileData);
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
