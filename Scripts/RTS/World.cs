namespace RTS;

public class ChunkPredefined
{
    public ChunkPredefined(Node parent, int chunkX, int chunkY, List<TileLayerPredefined> predefinedLayers)
    {
        var chunkParent = new Node2D();

        foreach (var layer in predefinedLayers)
        {
            //chunkParent.AddChild(GenerateMesh(chunkParent, chunkX, chunkY, layer));
        }

        parent.AddChild(chunkParent);
    }

    /*MeshInstance2D GenerateMesh(Node2D parent, int chunkX, int chunkY, TileLayerPredefined layer)
    {
        var size = World.ChunkSize;
        var vertices = new Vector3[4 * size * size];
        //var normals  = new Vector3[4 * size * size];
        var uvs      = new Vector2[4 * size * size];
        var colors   = new   Color[4 * size * size];
        var indices  = new     int[6 * size * size];

        for (int n = 0; n < colors.Length; n++)
            colors[n] = Colors.White;

        for (int m = 0; m < uvs.Length; m++)
            uvs[m] = new Vector2(0, 0);

        var image = tileLayer.TileSetImage;

        var tileOffset = World.TileSize / 2; // hard coded size
        var width = tileOffset * 2; // width

        var chunkSize = width * size;
        var chunkCoords = new Vector2(chunkX, chunkY);
        var tileChunkPos = chunkCoords * chunkSize; // (6400, 6400)
        var chunkPos = chunkCoords * size; // (100, 100)

        // Adding s adds hardcoded offset to align with godots grid
        // Also offset by (-chunkSize / 2) to center chunk
        var pos = new Vector3(
            x: tileOffset + (-chunkSize / 2) + tileChunkPos.X,
            y: tileOffset + (-chunkSize / 2) + tileChunkPos.Y,
            z: 0);

        var iIndex = 0;
        var vIndex = 0;

        var tileX = 0;
        var tileY = 0;

        var imageSize = image.GetSize();

        var tileWidth = World.TileSize / imageSize.X;
        var tileHeight = World.TileSize / imageSize.Y;

        // todo: finish this function
    }*/
}

public class TileLayerPredefined
{
    public Dictionary<Vector2I, Vector2I> Data { get; } = new();
    public Texture2D Image { get; set; }
}

public partial class World : Node
{
    // Static is convienant but arguably makes code more confusing to read / decouple
    public static World Instance { get; private set; }

    public static List<TileLayer> TileLayers { get; } = new();

    public static Dictionary<Vector2I, Chunk> Chunks { get; } = new();
    public static int ChunkSize { get; } = 10;
    public static int TileSize { get; } = 16;
    public static int SpawnRadius { get; } = 3;

    Node2D parentChunks;

    public override void _Ready()
    {
        Instance = this;

        parentChunks = new Node2D
        {
            Name = "Chunks"
        };
        AddChild(parentChunks);

        // DEBUG
        var predefinedChunk = GD.Load<PackedScene>("res://Scenes/Prefabs/test.tscn");
        var layer0 = 0;

        var tileLayer = new TileLayerPredefined();

        foreach (TileMap tileMap in predefinedChunk.Instantiate().GetChildren())
        {
            var source = tileMap.TileSet.GetSource(0) as TileSetAtlasSource;
            tileLayer.Image = source.Texture;

            foreach (var pos in tileMap.GetUsedCells(layer0))
            {
                var uvs = tileMap.GetCellAtlasCoords(layer0, pos);

                tileLayer.Data[pos] = uvs;
            }
        }

        SetupTileLayers();
        GenerateSpawn();
    }

    public void GenerateChunk(int x, int y)
    {
        var chunk = new Chunk(parentChunks, x, y);
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

        TileLayers.Add(new(
            zindex: -10,
            tileSetImagePath: "Sprites/basictiles.png",
            fnl: grassNoise,
            tileData: new()
            {
                { "grass_1", new TileData(new Vector2I(3, 1), 10f) },
                { "grass_2", new TileData(new Vector2I(0, 8), 10f) }
            }, 0f));

        TileLayers.Add(new(
            zindex: -9,
            tileSetImagePath: "Sprites/basictiles.png",
            fnl: treeNoise,
            tileData: new()
            {
                { "tree_1",  new TileData(new Vector2I(6, 4), 50f, true) }
            }, 30f));
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
