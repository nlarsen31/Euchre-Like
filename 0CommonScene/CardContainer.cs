using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Security;
using static GlobalProperties;

public partial class CardContainer : StaticBody2D, IComparable<CardContainer>
{
	// Signals
	[Signal]
	public delegate void CardSelectedEventHandler();

	private Suit _suit;
	public Suit Suit
	{
		get
		{
			if (_rank == Rank.jack)
			{
				if (Trump == Suit.DIAMONDS && _suit == Suit.HEARTS)
					return Suit.DIAMONDS;
				else if (Trump == Suit.HEARTS && _suit == Suit.DIAMONDS)
					return Suit.HEARTS;
				else if (Trump == Suit.CLUBS && _suit == Suit.SPADES)
					return Suit.CLUBS;
				else if (Trump == Suit.SPADES && _suit == Suit.CLUBS)
					return Suit.SPADES;
				else
					return _suit;
			}
			else
				return _suit;
		}
		set
		{
			_suit = value;
		}
	}
	private Rank _rank;
	public Rank Rank
	{
		get; set;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override string ToString()
	{
		return $"{SuitToString[(int)this.Suit]}_{RankToString[(int)this.Rank]}";
	}

	public void SetAnimation(Rank rank, Suit suit)
	{
		this.Suit = suit;
		this.Rank = rank;
		SetAnimation();
	}
	public void FlipDown()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "card_back";
	}

	public void SetAnimation()
	{
		string animName = RankToString[(int)this.Rank] + "_" + SuitToString[(int)this.Suit];
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = animName;
	}
	public void _on_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton input = (InputEventMouseButton)@event;
			if (input.Pressed && input.ButtonIndex == MouseButton.Left)
			{
				string param = this.ToString();
				// GD.Print($"{SuitToString[(int)this.Suit]}_{RankToString[(int)this.Rank]} was clicked");
				EmitSignal(SignalName.CardSelected, param);
			}
		}
	}

	public int CompareTo(CardContainer other)
	{
		int suitFactor = (int)Suit * 13;
		int rankOffset = (int)Rank;
		int comp = suitFactor + rankOffset;

		int OtherSuitFactor = (int)other.Suit * 13;
		int OtherRankFactor = (int)other.Rank;
		int otherComp = OtherSuitFactor + OtherRankFactor;

		return comp - otherComp;

		// return string.Compare(this.ToString(), other.ToString());
	}

}
