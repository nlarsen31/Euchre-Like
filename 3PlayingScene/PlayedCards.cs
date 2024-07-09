using Godot;
using System;

using static GlobalProperties;
using static GlobalMethods;
using System.Linq;
public partial class PlayedCards : Node2D
{
	private bool rightPlayed = false, leftPlayed = false,
		partnerPlayed = false, playerPlayed = false;
	public bool HaveAllPlayersPlayed
	{
		get
		{
			return rightPlayed && leftPlayed && partnerPlayed && playerPlayed;
		}
	}

	public void SetPlayedCards(CardContainer[] cardContainers)
	{
		if (cardContainers.Count() < 4) return;

		cardContainers[(int)Player.PLAYER] = playerCard;
		cardContainers[(int)Player.PARTNER] = partnerCard;
		cardContainers[(int)Player.RIGHT] = rightCard;
		cardContainers[(int)Player.LEFT] = leftCard;
	}

	public void resetPlayed()
	{
		playerPlayed = false;
		rightPlayed = false;
		leftPlayed = false;
		partnerPlayed = false;
	}

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
		Tuple<Rank, Suit> cardTup = GetSuitRankFromString(card);
		CardContainer cardContainer = null;
		switch (player)
		{
			case Player.PLAYER:
				playerPlayed = true;
				cardContainer = playerCard;
				break;
			case Player.RIGHT:
				rightPlayed = true;
				cardContainer = rightCard;
				break;
			case Player.LEFT:
				leftPlayed = true;
				cardContainer = leftCard;
				break;
			case Player.PARTNER:
				partnerPlayed = true;
				cardContainer = partnerCard;
				break;
			default:
				return;
		}
		cardContainer.Rank = cardTup.Item1;
		cardContainer.Suit = cardTup.Item2;

		cardContainer.SetAnimation();
		cardContainer.Visible = true;
	}
}
