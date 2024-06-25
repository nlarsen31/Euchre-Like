using Godot;
using System;
using System.Net.Security;
using System.Xml;
using static GlobalProperties;

public partial class CardDebugging : Node2D
{
	// exports
	[Export]
	private PackedScene CardContainer;
	private int cardWidth = 96;
	private int cardHeight = 128;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Suit[] suits = {
			Suit.HEARTS,
			Suit.DIAMONDS,
			Suit.SPADES,
			Suit.CLUBS};
		Rank[] ranks = {
			Rank.ace,
			Rank.two,
			Rank.three,
			Rank.four,
			Rank.five,
			Rank.six,
			Rank.seven,
			Rank.eight,
			Rank.nine,
			Rank.ten,
			Rank.jack,
			Rank.queen,
			Rank.king
		};

		int x = 200;
		int y = 200;
		int space = 10;
		foreach (Suit suit in suits) {
			foreach (Rank rank in ranks) {
				CardContainer card = (CardContainer)CardContainer.Instantiate();
				card.SetAnimation(rank, suit);
				card.Position = new Vector2(x,y);
				x += cardWidth + space;
				AddChild(card);
			}
			y += cardHeight + space;
			x = 200;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
