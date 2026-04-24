using Godot;

public partial class Bird : CharacterBody2D
{
    [Export] public float Gravity = 980f;
    [Export] public float FlapStrength = 300f;

    public bool Active { get; set; } = false;

    [Signal]
    public delegate void CrashedEventHandler();

    private bool crashed = false;

    public override void _PhysicsProcess(double delta)
    {
        if (!Active || crashed) return;
        
        Vector2 v = Velocity;
        v.Y += Gravity * (float)delta;

        if (Input.IsActionJustPressed("ui_accept"))
        {
            v.Y = -FlapStrength;
        }

        Velocity = v;
        MoveAndSlide();

        if (GetSlideCollisionCount() > 0)
        {
            crashed = true;
            EmitSignal(SignalName.Crashed);
        }
    }
}