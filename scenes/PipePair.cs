using Godot;
using System;

public partial class PipePair : CharacterBody2D
{
    [Signal]
    public delegate void DeathEventHandler();
    [Signal]
    public delegate void ScoreEventHandler();

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }

    private void OnTopPipeBodyEntered(PhysicsBody2D body)
    {
        if (body.Name == "Bird")
        {
            EmitSignal("Death");
        }
        // GD.Print("Top pipe body entered " + body.Name);
    }

    private void OnBottomPipeBodyEntered(PhysicsBody2D body)
    {
        if (body.Name == "Bird")
        {
            EmitSignal("Death");
        }
        // GD.Print("Bottom pipe body entered " + body.Name);
    }

    private void OnMiddlePipeBodyEntered(PhysicsBody2D body)
    {
        if (body.Name == "Bird")
        {
            EmitSignal("Score");
        }
        // GD.Print("Middle pipe body entered, " + body.Name);
    }
}
