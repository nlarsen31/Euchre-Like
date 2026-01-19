using System;
using Godot;
using static GlobalProperties;

public partial class Results : Node2D
{
   private string _LoosingString = "Better Luck Next time";
   private string _WinningString = "You won!";


   public override void _Ready()
   {
      Label resultLabel = GetNode<Label>("ResultLabel");
      if (CurrentWonGameState == WonGameState.Won)
      {
         resultLabel.Text = _WinningString;
      }
      else if (CurrentWonGameState == WonGameState.Lost)
      {
         resultLabel.Text = _LoosingString;
      }
      else
      {
         resultLabel.Text = "Game not finished properly.";
      }
   }
   public void _on_play_pressed()
   {
      GD.Print("PLAY PRESSED");
      GetTree().ChangeSceneToFile("res://2DraftScene/DraftScene.tscn");
      RequiredTricks = 3;
   }
}
