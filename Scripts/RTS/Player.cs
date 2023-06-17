namespace RTS;

public partial class Player : Entity
{
    public float Speed { get; set; } = 10;
    public float Friction { get; set; } = 0.1f;

    protected override State InitialState() => Move();

    State Move()
    {
        var state = new State("Move");

        state.Update = () =>
        {
            // Velocity is mutiplied by delta for us already
            Velocity += GUtils.GetMovementInput() * Speed;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction);
        };

        return state;
    }
}
