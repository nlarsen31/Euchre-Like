using Godot;
using System;
using System.Dynamic;
using System.Net.Security;
using static GlobalProperties;

public partial class CardContainer : StaticBody2D, IComparable<CardContainer>
{
	// Signals
	[Signal]
	public delegate void CardSelectedEventHandler();


	public Suit Suit;
	public Rank Rank{get; set;}

	public void SetSuit(Suit s)
	{
		this.Suit = s;
	}
	public void SetRank(Rank r)
	{
		this.Rank = r;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override string ToString()
    {
        return $"{SuitToString[(int)this.Suit]}_{RankToString[(int)this.Rank]}";
    }

    public void SetAnimation(Rank rank, Suit suit) {
		this.Suit = suit;
		this.Rank = rank;
		SetAnimation();
	}
	public void FlipDown()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "card_back";
	}

	public void SetAnimation() {
		string animName = RankToString[(int)this.Rank] + "_" + SuitToString[(int)this.Suit];
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = animName;
	}
	public void _on_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton input = (InputEventMouseButton) @event;
			if(input.Pressed && input.ButtonIndex == MouseButton.Left) 
			{
				string param = this.ToString();
				GD.Print($"{SuitToString[(int)this.Suit]}_{RankToString[(int)this.Rank]} was clicked");
				EmitSignal(SignalName.CardSelected, param);
			}
		}
	}

    public int CompareTo(CardContainer other)
    {
        return string.Compare(this.ToString(), other.ToString());
    }

}
