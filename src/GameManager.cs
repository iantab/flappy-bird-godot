using Godot;

public partial class GameManager : Node
{
	[Export] public PackedScene PipePairScene;
	[Export] public NodePath SpawnTimerPath;
	
	private Timer spawnTimer;
	private float lastGapY = 144f;
	
	public override void _Ready()
	{
		spawnTimer = GetNode<Timer>(SpawnTimerPath);
		spawnTimer.Timeout += OnSpawnTimerTimeout;
	}

	private void OnSpawnTimerTimeout()
	{
		float drift = (float)GD.RandRange(-30, 30);
		float newGapY = Mathf.Clamp(lastGapY + drift, 60, 220);
		lastGapY = newGapY;
		
		PipePair pair = PipePairScene.Instantiate<PipePair>();
		pair.Position = new Vector2(550, 0); // just off-screen right
		pair.Configure(newGapY);
		GetParent().AddChild(pair);
	}

	
}
