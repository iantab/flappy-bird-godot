using Godot;

public partial class GameManager : Node
{
	public enum GameState { Title, Countdown, Play, Score }
	public GameState State { get; private set; } = GameState.Title;
	
	[Export] public PackedScene PipePairScene;
	[Export] public NodePath SpawnTimerPath;
	[Export] public NodePath BirdPath;
	[Export] public NodePath TitleLabelPath;
	[Export] public NodePath MessageLabelPath;
	[Export] public NodePath ScoreLabelPath;
	
	private Timer spawnTimer;
	private Bird bird;
	private Label titleLabel;
	private Label messageLabel;
	private Label scoreLabel;
	private int score = 0;
	private float countdown = 3f;
	private float lastGapY = 144f;
	
	public override void _Ready()
	{
		spawnTimer = GetNode<Timer>(SpawnTimerPath);
		spawnTimer.Timeout += OnSpawnTimerTimeout;
		bird = GetNode<Bird>(BirdPath);
		titleLabel = GetNode<Label>(TitleLabelPath);
		messageLabel = GetNode<Label>(MessageLabelPath);
		scoreLabel = GetNode<Label>(ScoreLabelPath);
		bird.Crashed += OnBirdCrashed;
		EnterTitleState();
	}
	
	public override void _Process(double delta)
	{
		switch (State)
		{
			case GameState.Title:
				if (Input.IsActionJustPressed("ui_accept"))
					EnterCountdownState();
				break;
			case GameState.Countdown:
				countdown -= (float)delta;
				messageLabel.Text = Mathf.CeilToInt(countdown).ToString();
				if (countdown <= 0f) EnterPlayState();
				break;
			case GameState.Score:
				if (Input.IsActionJustPressed("ui_accept"))
					GetTree().ReloadCurrentScene(); // simple restart
				break;
		}
	}
	private void EnterTitleState()
	{
		State = GameState.Title;
		titleLabel.Visible = true;
		messageLabel.Text = "Press Enter";
		spawnTimer.Stop();
	}
	
	private void EnterCountdownState()
	{
		State = GameState.Countdown;
		titleLabel.Visible = false;
		countdown = 3f;
		bird.Active = true;
	}
	
	private void EnterPlayState()
	{
		State = GameState.Play;
		messageLabel.Text = "";
		spawnTimer.Start();
	}

	private void OnBirdCrashed()
	{
		State = GameState.Score;
		spawnTimer.Stop();
		messageLabel.Text = $"Score: {score}\nPress Enter to restart";
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
