namespace Template;

public partial class Test : TextureRect
{
    Vector2[] biggestCurvePoints;

    Image img;
    int size;

    int testIndex;

    public override void _Ready()
    {
        size = 128;

        // First lets create the biggest curve there will be
        biggestCurvePoints = CreateCurve(new Vector2(size - 1, size - 1), size / 2);

        testIndex = biggestCurvePoints.Length - 1;

        CreateSlash(biggestCurvePoints);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            // If the user is holding down the A key then move through the slash frames
            if (eventKey.Keycode == Key.A)
            {
                if (testIndex <= 0)
                    return;

                // Get one of the baked points from the biggest curve
                var point = biggestCurvePoints[testIndex];

                // This calculation is wrong. You can see the curve gets inverted
                // too early when the slash gets smaller

                // This calculation was eye balled through trial and error. It will
                // most likely fail when the size changes from 128 to something else.
                var curveReduction = (biggestCurvePoints.Length - testIndex) / 4;

                testIndex--;

                var points = CreateCurve(point, size / 2 - curveReduction);

                CreateSlash(points);
            }
        }
    }

    private Vector2[] CreateCurve(Vector2 finalPoint, int curvature)
    {
        var curve = new Curve2D();

        // The default bake interval is 5, this means only every 5 pixels a position
        // is calculated. So that is why this is set to 1, so we get a position for
        // every pixel.
        curve.BakeInterval = 1;

        // The first point is always at the top left
        var posPointA = new Vector2(0, 0);

        // The final point is usually near the bottom right
        var posPointB = finalPoint;

        // The curve only needs 2 points to be created
        curve.AddPoint(posPointA,
            @in: null,
            @out: new Vector2(curvature, 0));

        curve.AddPoint(posPointB,
            @in: new Vector2(0, -curvature),
            @out: null);

        return curve.GetBakedPoints();
    }

    private void CreateSlash(Vector2[] points)
    {
        img = Image.Create(size, size, true, Image.Format.Rgba8);

        // This is temporary, it is nice to have a black background to see how close
        // the pixels are to the bounds and to see where the pixel cut off is
        img.Fill(Colors.Black);

        // Note that a x < 2 and y < 20 looks good for the biggest slash but
        // every other smaller slash does not work nicely with these values.

        // Perhaps the x and y need to be changed dynamically based on 'i'
        // Something like y < (points.Length - i) / 8 seems to work
        for (int i = 0; i < points.Length; i++)
        {
            // How wide should the slash be?
            for (int x = 0; x < 2; x++)
            {
                // How long should the slash be?
                for (int y = 0; y < (points.Length - i) / 8; y++)
                {
                    var pos = new Vector2I((int)points[i].X + x, (int)points[i].Y + y);

                    // Calculate the transparency of the slash
                    // We subtract x and y to remove unwanted "line streaks"
                    var alpha = (float)(points.Length - i - x - y) / (points.Length);

                    SetColor(pos, new Color(1, 1, 1, Mathf.Max(alpha, 0)));
                }
            }
        }

        Texture = ImageTexture.CreateFromImage(img);
    }

    private void SetColor(Vector2 pos, Color color)
    {
        // If trying to draw a pixel outside of the images bounds then just disregard
        // that pixel
        if (pos.X >= 0 && pos.Y >= 0 && pos.X <= size - 1 && pos.Y <= size - 1)
            img.SetPixelv((Vector2I)pos, color);
    }
}
