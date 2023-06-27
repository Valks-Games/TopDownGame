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
        var swordRotAcc = 0.08f;
        var rotOffset = Mathf.Pi / 6;

        pivot.Rotation = Mathf.LerpAngle(pivot.Rotation, mouseDir.Angle() - rotOffset, swordRotAcc);

        if (Input.IsActionJustPressed("interact") && !tween.IsRunning())
        {
            tween.Create();

            // Swing backwards
            tween.Animate("rotation", pivot.Rotation - Mathf.Pi / 4,
                    duration: 0.4)
                .SetTrans(Tween.TransitionType.Sine);

            // Swing forwards
            tween.Animate("rotation", pivot.Rotation + Mathf.Pi / 4, 
                    duration: 0.2)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);
        }
    }
}
