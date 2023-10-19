using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void ShowMessage(string text)
    {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();
    }

    public void ShowMessage2(string text)
    {
        var message = GetNode<Label>("Message2");
        message.Text = text;
        message.Show();
    }

    async public void ShowGameOver(int highScore, bool newHighScore)
    {
        if (newHighScore)
        {
            var scoreLabel = GetNode<Label>("ScoreLabel");
            scoreLabel.Text =  highScore.ToString() + "\nNew High Score!";
            // Load rainbow.gdshader shader file
            var shader = ResourceLoader.Load<Shader>("res://scenes/rainbow.gdshader");
            scoreLabel.Material = new ShaderMaterial();
            (scoreLabel.Material as ShaderMaterial).Shader = shader;
            (scoreLabel.Material as ShaderMaterial).SetShaderParameter("strength", 0.65f);
            (scoreLabel.Material as ShaderMaterial).SetShaderParameter("speed", 0.75f);
            await ToSignal(GetTree().CreateTimer(1), "timeout");
        }

        ShowMessage("Game Over");

        await ToSignal(GetTree().CreateTimer(1), "timeout");

        ShowMessage2("Press space to restart");
        GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        // Reset shader material
        var scoreLabel = GetNode<Label>("ScoreLabel");
        scoreLabel.Material = null;
        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }

}
