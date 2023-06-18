using Godot;
using System.Drawing;

namespace RTS;

public partial class World : Node
{
    public static World Instance { get; private set; }
    public static Dictionary<string, Vector2I> Atlas { get; } = new()
    {
        { "grass_1", new Vector2I(3, 1) },
        { "grass_2", new Vector2I(0, 8) },
        { "tree_1", new Vector2I(6, 4) }
    };

    public static int ChunkSize { get; } = 4;
    public static int TileSize { get; } = 16;
    public static int SpawnChunkSize { get; } = 1;

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
        new Chunk(x, y, Grass, noise);
    }

    void GenerateSpawn()
    {
        for (int x = 0; x < SpawnChunkSize; x++)
        {
            for (int y = 0; y < SpawnChunkSize; y++)
            {
                new Chunk(x, y, Grass, noise);
            }
        }
    }
}
