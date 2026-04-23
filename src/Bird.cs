using Godot;

public partial class Bird : CharacterBody2D
{
    [Export] public float Gravity = 980f;
    [Export] public float FlapStrength = 300f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 v = Velocity;
        v.Y += Gravity * (float)delta;

        if (Input.IsActionJustPressed("ui_accept"))
        {
            v.Y = -FlapStrength;
        }

        Velocity = v;
        MoveAndSlide();
    }
}