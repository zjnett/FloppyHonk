using Godot;
using System;

public partial class Bird : CharacterBody2D
{
    [Signal]
    public delegate void GameOverEventHandler();

    [Export]
    private const float Gravity = 3000.0f;
    [Export]
    private const int JumpImpulse = 50000;

    private bool isInScreen = true;
    private bool isDead = false;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        // GD.Print("Is on screen: " + isInScreen);
        var velocity = Velocity;
        velocity.Y += (float)delta * Gravity;

        if (Input.IsActionJustPressed("jump") && !isDead)
        {
            // Play flap sound
            GetNode<AudioStreamPlayer>("FlapSound").Play();
            // Check if player is out of viewport, if not, don't apply
            if (isInScreen)
            {
                // Instantaneously zero out velocity, apply jump impulse
                velocity.Y = 0;
                velocity.Y -= (float)delta * JumpImpulse;
            }
        }

        Velocity = velocity;

        var motion = velocity * (float)delta;

        // Rotate bird based on velocity
        var rotation = 0.0f;
        if (Velocity.Y < 0)
        {
            // Upward impulse, rotate upwards, max of 50 degrees
            rotation = Mathf.Clamp(Mathf.Lerp(0, -50, -Velocity.Y / 1000), -50, 0);
        }
        else if (Velocity.Y > 0)
        {
            // Downward impulse, rotate downwards, max of 90 degrees
            rotation = Mathf.Clamp(Mathf.Lerp(0, 90, Velocity.Y / 1000), 0, 90);
        }
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = rotation;

        MoveAndCollide(motion);
    }

    public void StartScreen(Vector2 startPosition)
    {
        Position = startPosition;
        isDead = true;
        SetPhysicsProcess(false);
        // Play fly animation
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fly");
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    public void Start(Vector2 startPosition)
    {
        Position = startPosition;
        isDead = false;
        SetPhysicsProcess(true);
        Velocity = new Vector2(0, 0);
        // Play fly animation
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fly");
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    public void Die()
    {
        isDead = true;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void _OnVisibilityNotifier2DScreenExited()
    {
        isInScreen = false;
    }

    private void _OnVisibilityNotifier2DScreenEntered()
    {
        isInScreen = true;
    }

}
