namespace RTS;

public partial class Enemy : CharacterBody2D
{
	[Export] public Player Player { get; set; }

	NavigationAgent2D agent;
	GTimer timer;

	public override void _Ready()
	{
		agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		timer = new(this, 1000);
		timer.Start();
		timer.Loop = true;
		timer.Finished += () => MakePath();
	}

	public override void _PhysicsProcess(double delta)
	{
		var dir = ToLocal(agent.GetNextPathPosition()).Normalized();
		Velocity = dir * 20;
		MoveAndSlide();
	}

	void MakePath()
	{
		GD.Print("calculating path");
		agent.TargetPosition = Player.GlobalPosition;
	}
}
