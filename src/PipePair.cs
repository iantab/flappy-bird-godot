using Godot;

public partial class PipePair : Node2D
{
	[Export] public float Speed = 60f;
	[Export] public float GapHeight = 90f;
	
	[Export] public NodePath UpperPipePath;
	[Export] public NodePath LowerPipePath;
	[Export] public NodePath ScoreZonePath;
	
	public bool Scored { get; set; } = false;

	[Signal]
	public delegate void ScoredByBirdEventHandler();

	public override void _Ready()
	{
		var zone = GetNode<Area2D>(ScoreZonePath);
		zone.BodyEntered += OnScoreZoneEntered;
	}

	private void OnScoreZoneEntered(Node2D body)
	{
		if (body is not Bird) return;
		if (Scored) return;
		Scored = true;
		EmitSignal(SignalName.ScoredByBird);
	}

	public void Configure(float gapCenterY)
	{
		var upper = GetNode<Node2D>(UpperPipePath);
		var lower = GetNode<Node2D>(LowerPipePath);

		upper.Position = new Vector2(upper.Position.X, gapCenterY - GapHeight / 2f);
		lower.Position = new Vector2(lower.Position.X, gapCenterY + GapHeight / 2f);
	}

	public override void _PhysicsProcess(double delta)
	{
		Position -= new Vector2(Speed * (float)delta, 0);

		if (Position.X < -100)
		{
			QueueFree();
		}
	}
}
