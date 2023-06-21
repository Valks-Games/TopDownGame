namespace RTS;

public partial class World : Node
{
	public static World Instance { get; private set; }
	public static Dictionary<string, AtlasWeight> Atlas { get; } = new()
	{
		{ "grass_1", new AtlasWeight(new Vector2I(3, 1), 0.3f) },
		{ "grass_2", new AtlasWeight(new Vector2I(0, 8), 1f) },
		{ "tree_1",  new AtlasWeight(new Vector2I(6, 4), 0.1f) }
	};

	public static int ChunkSize { get; } = 10;
	public static int TileSize { get; } = 16;
	public static int SpawnRadius { get; } = 3;
	public static Dictionary<Vector2I, bool> ChunkGenerated { get; } = new();

	[Export] public TileMap Grass { get; set; }
	[Export] public TileMap Trees { get; set; }

	Noise noise;

	public override void _Ready()
	{
		Instance = this;

		noise = new FastNoiseLite
		{
			Frequency = 0.1f
		};

		GenerateSpawn();
	}

	public void GenerateChunk(int x, int y)
	{
		World.ChunkGenerated[new Vector2I(x, y)] = true;
		new Chunk(x, y, Grass, noise);
	}

	void GenerateSpawn()
	{
		for (int x = -SpawnRadius; x <= SpawnRadius; x++)
		{
			for (int y = -SpawnRadius; y <= SpawnRadius; y++)
			{
				new Chunk(x, y, Grass, noise);
			}
		}
	}
}
