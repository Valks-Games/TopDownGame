namespace RTS;

public partial class World : Node
{
    // Static is convienant but arguably makes code more confusing to read / decouple
    public static World Instance { get; private set; }

    public static List<TileLayer> TileLayers { get; } = new();
    public static Dictionary<Vector2I, Chunk> Chunks { get; } = new();
    public static int ChunkSize { get; } = 10;
    public static int TileSize { get; } = 16;
    public static int SpawnRadius { get; } = 3;

    public override void _Ready()
    {
        Instance = this;

        SetupTileLayers();
        //GenerateChunk(0, 0);
        GenerateSpawn();
    }

    public void GenerateChunk(int x, int y)
    {
        var chunk = new Chunk(x, y);
        Chunks[new Vector2I(x, y)] = chunk;
    }

    void SetupTileLayers()
    {
        var grassNoise = new FastNoiseLite
        {
            Frequency = 0.1f
        };

        var treeNoise = new FastNoiseLite
        {
            Frequency = 0.05f,
            Offset = new Vector3(1000, 0, 0)
        };

        var grassLayer = new TileLayer(GetNode<TileMap>("Grass"), grassNoise, new Dictionary<string, TileData>
        {
            { "grass_1", new TileData(new Vector2I(3, 1), 33) },
            { "grass_2", new TileData(new Vector2I(0, 8), 33) }
        }, 0);

        var treeLayer = new TileLayer(GetNode<TileMap>("Trees"), treeNoise, new Dictionary<string, TileData>
        {
            { "tree_1", new TileData(new Vector2I(6, 4), 40) }
        }, 60);

        TileLayers.Add(grassLayer);
        TileLayers.Add(treeLayer);
    }

    void GenerateSpawn()
    {
        for (int x = -SpawnRadius; x <= SpawnRadius; x++)
        {
            for (int y = -SpawnRadius; y <= SpawnRadius; y++)
            {
                GenerateChunk(x, y);
            }
        }
    }
}
