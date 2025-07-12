using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using Godot;
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
				if (CurrentTrump == Suit.DIAMONDS && _suit == Suit.HEARTS)
					return Suit.DIAMONDS;
				else if (CurrentTrump == Suit.HEARTS && _suit == Suit.DIAMONDS)
					return Suit.HEARTS;
				else if (CurrentTrump == Suit.CLUBS && _suit == Suit.SPADES)
					return Suit.CLUBS;
				else if (CurrentTrump == Suit.SPADES && _suit == Suit.CLUBS)
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
	public Suit RealSuit
	{
		get { return _suit; }
	}

	public bool IsLeft
	{
		get
		{
			if (this.Suit == CurrentTrump && _rank == Rank.jack && _suit != CurrentTrump) return true;
			return false;
		}
	}
	public bool IsRight
	{
		get
		{
			if (this.Suit == CurrentTrump && _rank == Rank.jack && _suit == CurrentTrump) return true;
			return false;
		}
	}

	private Rank _rank;
	public Rank Rank
	{
		get
		{
			return _rank;
		}
		set
		{
			_rank = value;
		}
	}

	private bool _selectable;
	public bool Selectable
	{
		get { return _selectable; }
		set
		{
			if (value)
			{
				SetBorderColor("red");
				if (IsMouseInside())
					SetBorderColor("yellow");
			}
			else
			{
				SetBorderColor("black");
			}
			_selectable = value;
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
	}
	public override string ToString()
	{
		return $"{SuitToString[(int)_suit]}_{RankToString[(int)_rank]}";
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
		string animName = RankToString[(int)this.Rank] + "_" + SuitToString[(int)_suit];
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = animName;
	}
	public void SetBorderColor(string color)
	{
		AnimatedSprite2D borderSprite = GetNode<AnimatedSprite2D>("BorderFrames");
		if (color == "black")
		{
			borderSprite.Visible = false;
		}
		else
		{
			borderSprite.Visible = true;
			borderSprite.Animation = color;
		}
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
	public void OnMouseEntered()
	{
		if (Selectable)
			SetBorderColor("yellow");
	}
	public void OnMouseExited()
	{
		if (Selectable)
			SetBorderColor("red");
	}

	public int CompareTo(CardContainer other)
	{
		// 14 to contain left in the suits

		int suitFactor = (int)Suit * 15;
		int rankOffset = (int)Rank;
		if (this.IsLeft) rankOffset = 13;
		if (this.IsRight) rankOffset = 14;
		int comp = suitFactor + rankOffset;

		int OtherSuitFactor = (int)other.Suit * 15;
		int OtherRankFactor = (int)other.Rank;
		if (other.IsLeft) OtherRankFactor = 13;
		if (other.IsRight) OtherRankFactor = 14;

		int otherComp = OtherSuitFactor + OtherRankFactor;

		return comp - otherComp;
	}

	// comparison operators DOES NOT consider what suit is lead.
	// Comparison operators DO consider what suit is trump

	public static bool operator <(CardContainer card1, CardContainer card2)
	{
		if (card1.Suit == CurrentTrump && card2.Suit != CurrentTrump)
			return false;
		else if (card1.Suit != CurrentTrump && card2.Suit == CurrentTrump)
			return true;
		else if (card1.Suit == CurrentTrump && card2.Suit == CurrentTrump)
		{
			if (card1.Rank == Rank.jack && card2.Rank != Rank.jack)
				return false;
			else if (card1.Rank != Rank.jack && card2.Rank == Rank.jack)
				return true;
			else if (card1.Rank == Rank.jack && card2.Rank == Rank.jack)
				return card2.IsRight; // If card2 is right, < is true
			else // Neither are jacks

				return card1.Rank < card2.Rank;
		}
		else if (card1.Suit != CurrentTrump && card2.Suit != CurrentTrump)
		{
			return card1.Rank < card2.Rank;
		}
		return false;
	}
	public static bool operator >(CardContainer card1, CardContainer card2)
	{
		if (card1.Rank == card2.Rank && card1.Suit == card2.Suit
			&& card1.IsRight == card2.IsRight && card1.IsLeft == card2.IsLeft)
			return false;
		return card2 < card1;
	}

	public bool IsMouseInside()
	{
		Vector2 relative = GetLocalMousePosition();
		if (Math.Abs(relative.X) < CARD_WIDTH / 2 && Math.Abs(relative.Y) < CARD_HEIGHT / 2)
			return true;
		return false;
	}
}
