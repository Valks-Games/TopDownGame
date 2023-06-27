namespace RTS;

public partial class Sword : Sprite2D
{
    Node2D center;
    GTween tween;

    public override void _Ready()
    {
        center = GetParent<Node2D>();
        tween = new GTween(center);
        tween.Create();
        tween.Pause();
    }

    public override void _PhysicsProcess(double delta)
    {
        var mouseDir = (GetGlobalMousePosition() - center.GlobalPosition).Normalized();
        center.Rotation = Mathf.LerpAngle(center.Rotation, mouseDir.Angle(), 0.08f);

        var rot = center.Rotation;
        if (Input.IsActionJustPressed("interact") && !tween.IsRunning())
        {
            tween = new GTween(center);
            tween.Create();
            // swing forwards
            tween.Animate("rotation", rot + Mathf.Pi / 2, 
                    duration: .3)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);
        }
    }
}
