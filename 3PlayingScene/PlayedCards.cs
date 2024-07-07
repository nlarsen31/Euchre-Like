using Godot;
using System;

using static GlobalProperties;
using static GlobalMethods;
public partial class PlayedCards : Node2D
{
	CardContainer playerCard, rightCard, partnerCard, leftCard;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerCard = GetNode<CardContainer>("PlayerCard");
		rightCard = GetNode<CardContainer>("RightCard");
		partnerCard = GetNode<CardContainer>("PartnerCard");
		leftCard = GetNode<CardContainer>("LeftCard");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowAndSetCard(string card, Player player)
	{
		switch (player)
		{
			case Player.PLAYER:
				Tuple<Rank, Suit> cardTup = GetSuitRankFromString(card);
				playerCard.Visible = true;
				playerCard.Rank = cardTup.Item1;
				playerCard.Suit = cardTup.Item2;
				playerCard.SetAnimation();
				break;
		}
	}
}
