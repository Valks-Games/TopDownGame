namespace Template;

public partial class Test : TextureRect
{
    public override void _Ready()
    {
        CreateSlash(128, 50);
    }

    private void CreateSlash(int size, int padding)
    {
        size += padding;

        var path = new Path2D();
        var curve = new Curve2D();
        curve.BakeInterval = 1;

        var posPointA = new Vector2(padding / 2, padding / 2);
        var posPointB = new Vector2(size - 1 - padding / 2, size - 1 - padding / 2);

        curve.AddPoint(posPointA,
            @in: null,
            @out: new Vector2(size / 2, 0));

        curve.AddPoint(posPointB,
            @in: new Vector2(0, -size / 2),
            @out: null);

        path.Curve = curve;

        var points = curve.GetBakedPoints();

        var img = Image.Create(size, size, true, Image.Format.Rgba8);

        img.Fill(Colors.Black);

        for (int i = 0; i < points.Length; i++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -10; y <= 10; y++)
                {
                    var pos = new Vector2I((int)points[i].X + x, (int)points[i].Y + y);

                    var alpha = (float)(points.Length - i - x - y) / (points.Length);

                    img.SetPixelv(pos, new Color(1, 1, 1, Mathf.Max(alpha, 0)));
                }
            }
        }

        Texture = ImageTexture.CreateFromImage(img);
    }
}
