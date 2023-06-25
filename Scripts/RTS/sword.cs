using Godot;
using System;

public partial class sword : Sprite2D
{
    private Node2D center;
    private int dir;
    
    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        center = GetParent<Node2D>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
        center.LookAt(GetGlobalMousePosition());
        //center.RotationDegrees -= 45 * dir;
        var mouseDir = (GetGlobalMousePosition() - center.GlobalPosition).Normalized();
        dir = Mathf.Sign(mouseDir.X);
        if (mouseDir.X < 0)
            center.Scale = new Vector2(1, -1);
        else
            center.Scale = new Vector2(1, 1);
        var rot = center.Rotation;
        if (Input.IsActionJustPressed("interact"))
        {
            var tween = new GTween(center);
            tween.Create();
            // swing forwards
            tween.Animate("rotation", rot + Mathf.Pi / 2 * dir, 
                    duration: .3)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);

            // go back to starting pos
            tween.Animate("rotation", rot, 
                    duration: .5)
                .SetTrans(Tween.TransitionType.Sine);
        }
    }
}
