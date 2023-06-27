using Godot;
using System;

public partial class Sword : Sprite2D
{
    private Node2D center;
    private int dir;
    private GTween tween;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        center = GetParent<Node2D>();
        tween = new GTween(center);
        tween.Create();
        tween.Pause();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
        //center.LookAt(GetGlobalMousePosition());
        //center.RotationDegrees -= 45 * dir;
        var mouseDir = (GetGlobalMousePosition() - center.GlobalPosition).Normalized();
        dir = Mathf.Sign(mouseDir.X);
        center.Rotation = Mathf.LerpAngle(center.Rotation, mouseDir.Angle(), 0.08f);
        // if (!tween.IsRunning())
        // {
        //     if (mouseDir.X < 0)
        //         center.Scale = new Vector2(1, -1);
        //     else
        //         center.Scale = new Vector2(1, 1);
        // }

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
