using Godot;
using System;
using static GlobalProperties;

#if DEBUG
using System.Reflection;
using Chickensoft.GoDotTest;
#endif

public partial class start_scene : Node2D
{
#if DEBUG
	public TestEnvironment Environment = default!;
#endif
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
#if DEBUG
		// If this is a debug build, use GoDotTest to examine the
		// command line arguments and determine if we should run tests.
		Environment = TestEnvironment.From(OS.GetCmdlineArgs());
		if (Environment.ShouldRunTests)
		{
			CallDeferred("RunTests");
			return;
		}
#endif
		// If we don't need to run tests, we can just switch to the game scene.
		GetTree().ChangeSceneToFile("res://src/start_scene.tscn");
	}

#if DEBUG
	private void RunTests()
	  => _ = GoTest.RunTests(Assembly.GetExecutingAssembly(), this, Environment);
#endif

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
