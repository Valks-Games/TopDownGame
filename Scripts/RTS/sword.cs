namespace RTS;

public partial class Sword : Sprite2D
{
    Node2D pivot;
    GTween tween;

    public override void _Ready()
    {
        pivot = GetParent<Node2D>();
        tween = new GTween(pivot);
        tween.Create();
        tween.Pause();
    }

    public override void _PhysicsProcess(double delta)
    {
        var mouseDir = (GetGlobalMousePosition() - pivot.GlobalPosition).Normalized();
        pivot.Rotation = Mathf.LerpAngle(pivot.Rotation, mouseDir.Angle(), 0.08f);

        var rot = pivot.Rotation;
        if (Input.IsActionJustPressed("interact") && !tween.IsRunning())
        {
            tween.Create();
            // swing forwards
            tween.Animate("rotation", rot + Mathf.Pi / 2, 
                    duration: .3)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);
        }
    }
}
