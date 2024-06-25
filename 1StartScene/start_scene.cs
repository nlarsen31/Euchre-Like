using Godot;
using System;
using static GlobalProperties;


public partial class start_scene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalGamePhase = Phase.loading;
		GD.Print("Hello World!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_play_pressed()
	{
		GD.Print("PLAY PRESSED");
		GetTree().ChangeSceneToFile("res://2DraftScene/DraftScene.tscn");
	}

	public void _on_debug_pressed()
	{
		GD.Print("Debug Cards pressed");
		GetTree().ChangeSceneToFile("res://0CommonScene/CardDebugging.tscn");
	}
}
