using System.Drawing;

namespace RTS;

public partial class World : Node
{
    [Export] public TileMap Grass { get; set; }
    [Export] public TileMap Trees { get; set; }

    Dictionary<string, Vector2I> atlas = new()
    {
        { "grass_1", new Vector2I(3, 1) },
        { "grass_2", new Vector2I(0, 8) },
        { "tree_1", new Vector2I(6, 4) }
    };

    public override void _Ready()
    {
        var size = 100;

        var treeNoise = new FastNoiseLite
        {
            Frequency = 0.06f
        };

        var grassNoise = new FastNoiseLite
        {
            Frequency = 0.3f,
            Offset = new Vector3(1000, 0, 0)
        };

        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                GenGrass(x, y, grassNoise);
                GenTree(x, y, treeNoise);
            }
    }

    void GenGrass(int x, int y, Noise noise)
    {
        if (noise.GetNoise2D(x, y) > 0.3f)
        {
            SetCell(Grass, x, y, atlas["grass_2"]);
        }
        else
        {
            SetCell(Grass, x, y, atlas["grass_1"]);
        }
    }

    void GenTree(int x, int y, Noise noise)
    {
        if (noise.GetNoise2D(x, y) > 0.3f)
        {
            SetCell(Trees, x, y, atlas["tree_1"]);
        }
    }

    void SetCell(TileMap tileMap, int x, int y, Vector2I type) =>
        tileMap.SetCell(0, new Vector2I(x, y), 0, type);
}
