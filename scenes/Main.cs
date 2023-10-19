using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene PipesScene { get; set; }
    private int _score = 0;
    private int _high_score = 0;
    private bool _new_high_score = false;
    public override void _Ready()
    {
        setGroundScrolling(true);

        // Show bird at start position
        var player = GetNode<Bird>("Bird");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.StartScreen(startPosition.Position);

        // Show intro HUD
        var hud = GetNode<HUD>("HUD");
        hud.ShowMessage("Floppy Honk");
        hud.ShowMessage2("Press space to start");
        hud.StartGame += NewGame;
    }

    public void NewGame()
    {
        _score = 0;

        // Clear HUD messages
        var hud = GetNode<HUD>("HUD");
        hud.ShowMessage("");
        hud.ShowMessage2("");
        hud.UpdateScore(_score);

        // Clear all pipes
        foreach (PipePair pipe in GetTree().GetNodesInGroup("Pipes"))
        {
            pipe.QueueFree();
        }

        setGroundScrolling(true);

        var player = GetNode<Bird>("Bird");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
        player.Show();

        GetNode<Timer>("PipeSpawnTimer").Start();
    }

    public void GameOver()
    {
        var player = GetNode<Bird>("Bird");
        player.Die();
        setGroundScrolling(false);
        GetNode<Timer>("PipeSpawnTimer").Stop();
        foreach (PipePair pipe in GetTree().GetNodesInGroup("Pipes"))
        {
            pipe.Velocity = new Vector2(0, 0); // Stop all pipes
        }
        // Play death sound
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        // Show Game Over HUD
        GetNode<HUD>("HUD").ShowGameOver(_high_score, _new_high_score);
        _new_high_score = false;
    }

    private void setGroundScrolling(bool isScrolling)
    {
        var ground = GetNode<Sprite2D>("GroundTexture");
        if (isScrolling)
        {
            (ground.Material as ShaderMaterial).SetShaderParameter("Speed", 0.3);
        }
        else
        {
            (ground.Material as ShaderMaterial).SetShaderParameter("Speed", 0.0);
        }
    }

    private void OnPipeSpawnTimerTimeout()
    {
        PipePair pair = PipesScene.Instantiate<PipePair>();

        pair.Death += OnPipePairDeath;
        pair.Score += OnPipePairScore;

        var spawnPosition = GetNode<Marker2D>("PipeSpawnPosition");
        // Adjust vertical component of spawn position by random value
        Vector2 randomPosition = new Vector2(spawnPosition.Position.X, spawnPosition.Position.Y + GD.RandRange(-300, 300));
        pair.Position = randomPosition;
        // GD.Print("Spawn position: " + randomPosition);

        var velocity = new Vector2(-300, 0);
        pair.Velocity = velocity;

        AddChild(pair);

        pair.AddToGroup("Pipes");
    }
    
    private void OnPipePairScore()
    {
        // Play score sound
        GetNode<AudioStreamPlayer>("ScoreSound").Play();
        _score++;
        if (_score > _high_score)
        {
            _new_high_score = true;
            _high_score = _score;
        }
        GetNode<HUD>("HUD").UpdateScore(_score);
        GD.Print("Score: " + _score);
    }

    private void OnPipePairDeath()
    {
        GameOver();
    }

    private void OnGroundBodyEntered(PhysicsBody2D body)
    {
        if (body.Name == "Bird")
        {
            GameOver();
        }
    }
}
