namespace RTS;

public partial class Player : CharacterBody2D
{
    public float Speed { get; set; } = 10;
    public float Friction { get; set; } = 0.1f;

    public override void _PhysicsProcess(double delta)
    {
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
