using Godot;

public partial class Scroller : ParallaxBackground
{
    [Export] public float BackgroundSpeed = 30f;
    [Export] public float GroundSpeed = 60f;

    [Export] public NodePath BackgroundLayerPath;
    [Export] public NodePath GroundLayerPath;

    public override void _Ready()
    {
        var backgroundLayer = GetNode<ParallaxLayer>(BackgroundLayerPath);
        var groundLayer = GetNode<ParallaxLayer>(GroundLayerPath);
        backgroundLayer.MotionScale = new Vector2(BackgroundSpeed / GroundSpeed, 1);
        groundLayer.MotionScale = new Vector2(1, 1);
    }

    public override void _Process(double delta)
    {
        ScrollOffset -= new Vector2(GroundSpeed * (float)delta, 0);
    }
}
