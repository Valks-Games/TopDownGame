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
        timerDontCheck = new(this);
        timerDontCheck.Finished += () => dontCheck = false;
        label = new GLabel();
        label.Visible = ShowStates;
        label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.CenterBottom);
        AddChild(label);

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

    protected void DontCheck(float delay = 0.1f)
    {
        dontCheck = true;
        timerDontCheck.Start(delay * 1000);
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
