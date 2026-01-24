using System;
using System.Collections.Generic;
using Godot;

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
			return _tricksWon;
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

	AnimatedSprite2D[] _WonTrickChecks = new AnimatedSprite2D[13];
	AnimatedSprite2D[] _RequiredTricksChecks = new AnimatedSprite2D[13];
	AnimatedSprite2D[] _TricksLeftChecks = new AnimatedSprite2D[13];
	public override void _Ready()
	{
		_Required = GetNode<Label>("Required");
		_TricksLeft = GetNode<Label>("TricksWon");
		_TricksWon = GetNode<Label>("TricksLeft");
		_tricksWon = 0;
		for (int i = 0; i < 13; i++)
		{
			_WonTrickChecks[i] = GetNode<AnimatedSprite2D>($"won_{i + 1}");
			_RequiredTricksChecks[i] = GetNode<AnimatedSprite2D>($"req_{i + 1}");
			_TricksLeftChecks[i] = GetNode<AnimatedSprite2D>($"left_{i + 1}");
		}
	}

	public void SetRequired(int iRequiredTricksLeft)
	{
		for (int i = 0; i < iRequiredTricksLeft; i++)
		{

			_RequiredTricksChecks[i].Visible = true;
		}
		for (int i = iRequiredTricksLeft; i < 13; i++)
		{
			_RequiredTricksChecks[i].Visible = false;
		}
	}
	public void SetTricksLeft(int iTricksLeft)
	{
		GD.Print($"Setting tricks left to {iTricksLeft}");
		for (int i = 0; i < iTricksLeft; i++)
		{
			GD.Print($"  Making trick left {i} visible");
			_TricksLeftChecks[i].Visible = true;
		}
		for (int i = iTricksLeft; i < 13; i++)
		{
			GD.Print($"  Making trick left {i} invisible");
			_TricksLeftChecks[i].Visible = false;
		}
	}
	public void SetTricksWon(int iTricksWon)
	{
		for (int i = 0; i < iTricksWon; i++)
		{
			_WonTrickChecks[i].Visible = true;
		}
		for (int i = iTricksWon; i < 13; i++)
		{
			_WonTrickChecks[i].Visible = false;
		}
	}
	public void Reset(int iRequiredTricksLeft)
	{
		TricksWon = 0;
		TricksLeft = 13;
		TricksRequired = iRequiredTricksLeft;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
