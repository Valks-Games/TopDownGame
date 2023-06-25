namespace Template.TopDown2D;

public partial class Player : CharacterBody2D
{
	public float Speed    { get; set; } = 50;
	public float Friction { get; set; } = 0.1f;

	private Node2D meleePivot;

	public override void _Ready()
	{
		meleePivot = GetNode<Node2D>("MeleePivot");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			var tween = new GTween(meleePivot);
			tween.Create();
			tween.Animate("rotation", Mathf.Pi / 2, 10);
		}

		MoveAndSlide();

		// Velocity is mutiplied by delta for us already
		Velocity += GetMovementInput() * Speed;
		Velocity = Velocity.Lerp(Vector2.Zero, Friction);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			//GD.Print(motion.Position);
		}
	}

	public Vector2 GetMovementInput(string prefix = "")
	{
		if (!string.IsNullOrWhiteSpace(prefix))
			prefix += "_";

		// GetActionStrength(...) supports controller sensitivity
		var inputHorz = Input.GetActionStrength($"{prefix}move_right") - Input.GetActionStrength($"{prefix}move_left");
		var inputVert = Input.GetActionStrength($"{prefix}move_down") - Input.GetActionStrength($"{prefix}move_up");

		// Normalize vector to prevent fast diagonal strafing
		return new Vector2(inputHorz, inputVert).Normalized();
	}
}
