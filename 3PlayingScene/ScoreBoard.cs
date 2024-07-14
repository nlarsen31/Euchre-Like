using Godot;
using System;

public partial class ScoreBoard : Node2D
{
	// Called when the node enters the scene tree for the first time.
	Label _Required;
	Label _TricksLeft;
	Label _TricksWon;
	private int _tricksWon;
	public int TricksWon
	{
		get
		{
			return _tricksLeft;
		}
		set
		{
			_tricksWon = value;
			SetTricksWon(_tricksWon);
		}
	}
	private int _tricksLeft;
	public int TricksLeft
	{
		get
		{
			return _tricksLeft;
		}
		set
		{
			_tricksLeft = value;
			SetTricksLeft(_tricksLeft);
		}
	}
	private int _tricksRequired;
	public int TricksRequired
	{
		get
		{
			return _tricksRequired;
		}
		set
		{
			_tricksRequired = value;
			SetRequired(_tricksRequired);
		}
	}
	public override void _Ready()
	{
		_Required = GetNode<Label>("Required");
		_TricksLeft = GetNode<Label>("TricksWon");
		_TricksWon = GetNode<Label>("TricksLeft");
		_tricksWon = 0;

	}

	public void SetRequired(int iRequiredTricksLeft)
	{
		_Required.Text = "Required Tricks: " + iRequiredTricksLeft;
	}
	public void SetTricksLeft(int iTricksLeft)
	{
		_TricksLeft.Text = "Tricks left: " + iTricksLeft;
	}
	public void SetTricksWon(int iTricksWon)
	{
		_TricksWon.Text = "Tricks won: " + iTricksWon;
	}
	public void Reset(int iRequiredTricksLeft)
	{
		TricksLeft = 13;
		TricksRequired = iRequiredTricksLeft;
		TricksWon = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
