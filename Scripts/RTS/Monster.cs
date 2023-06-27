namespace RTS;

public abstract partial class Monster : Entity
{
    [Export] public float DetectionRadius { get; set; } = 80;

    protected Player player;

    public override void _Ready()
    {
        CreateDetectionArea();
        base._Ready();
    }

    void CreateDetectionArea()
    {
        var area = GUtils.CreateAreaCircle(this, DetectionRadius, "ff001300");

        area.BodyEntered += body => 
        {
            if (body is Player player)
                this.player = player;
        };

        area.BodyExited += body =>
        {
            if (body is Player)
                this.player = null;
        };
    }
}
