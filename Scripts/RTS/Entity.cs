namespace RTS;

public abstract partial class Entity : CharacterBody2D
{
    [Export] public bool ShowStates { get; set; }
    [Export] public bool PrintStates { get; set; }

    protected AnimatedSprite2D sprite;
    protected bool dontCheck;

    Label label;
    GTimer timerDontCheck;
    State curState;

    public override void _Ready()
    {
        label = GetNode<Label>("DebugLabel");
        label.Visible = ShowStates;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        Init();

        curState = InitialState();
        curState.Enter();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        Update();

        curState.Update();
        curState.Transitions();
    }

    protected abstract State InitialState();

    protected void DontCheck(double delay = 0.1)
    {
        dontCheck = true;
        timerDontCheck = new(this, delay * 1000);
        timerDontCheck.Finished += () => dontCheck = false;
        timerDontCheck.Start();
    }

    public void SwitchState(State newState)
    {
        curState.Exit();

        timerDontCheck?.Stop();
        dontCheck = false;

        newState.Enter();
        curState = newState;

        if (PrintStates)
            Logger.Log(newState);

        label.Text = newState.ToString();
    }

    protected virtual void Init() { }
    protected virtual void Update() { }
}
