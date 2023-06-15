namespace MyGame;

public partial class Slime : Sprite2D
{
    FastNoiseLite fnl;
    float x;

    public override void _Ready()
    {
        fnl = new FastNoiseLite();
    }

    public override void _PhysicsProcess(double delta)
    {
        var s = fnl.GetNoise1D(x) * 100;

        x += 0.01f;

        //var s = 1;
        Offset = new Vector2((float)GD.RandRange(-s, s), (float)GD.RandRange(-s, s));
    }
}
