using Godot;

public partial class Bird : CharacterBody2D
{
    [Export] public float Gravity = 980f;
    [Export] public float FlapStrength = 300f;

    [Export] public NodePath JumpPlayerPath;
    [Export] public NodePath HurtPlayerPath;

    private AudioStreamPlayer jumpPlayer;
    private AudioStreamPlayer hurtPlayer;

    public bool Active { get; set; } = false;

    [Signal]
    public delegate void CrashedEventHandler();

    private bool crashed = false;

    public override void _Ready()
    {
        jumpPlayer = GetNode<AudioStreamPlayer>(JumpPlayerPath);
        hurtPlayer = GetNode<AudioStreamPlayer>(HurtPlayerPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Active || crashed) return;
        
        Vector2 v = Velocity;
        v.Y += Gravity * (float)delta;

        if (Input.IsActionJustPressed("ui_accept"))
        {
            v.Y = -FlapStrength;
            jumpPlayer.Play();
        }

        Velocity = v;
        MoveAndSlide();

        if (GetSlideCollisionCount() > 0)
        {
            crashed = true;
            hurtPlayer.Play();
            EmitSignal(SignalName.Crashed);
        }
    }
}