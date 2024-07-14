using Godot;
using System;

using static GlobalProperties;
using static GlobalMethods;
using System.Linq;
public partial class PlayedCards : Node2D
{
	private bool rightPlayed = false, leftPlayed = false,
		partnerPlayed = false, playerPlayed = false;
	Player _leadPlayer = Player.UNDEFINED;
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

		static int CountTrue(params bool[] args)
		{
			return args.Count(t => t);
		}

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
		cardContainer.SetBorderColor("black");

		// If we are the first card played highlight
		if (CountTrue(playerPlayed, rightPlayed, leftPlayed, partnerPlayed) == 1)
		{
			cardContainer.SetBorderColor("yellow");
			_leadPlayer = player;
		}
	}

	public void ClearCards()
	{
		resetPlayed();
		_leadPlayer = Player.UNDEFINED;
		playerCard.Visible = false;
		rightCard.Visible = false;
		leftCard.Visible = false;
		partnerCard.Visible = false;
	}


	public Player GetLeadPlayer()
	{
		return _leadPlayer;
	}
	public Suit GetLeadSuit()
	{
		CardContainer cardContainer;
		if (_leadPlayer == Player.PLAYER) cardContainer = playerCard;
		else if (_leadPlayer == Player.RIGHT) cardContainer = rightCard;
		else if (_leadPlayer == Player.PARTNER) cardContainer = partnerCard;
		else if (_leadPlayer == Player.LEFT) cardContainer = leftCard;
		else cardContainer = null;

		if (null == cardContainer)
			return Suit.UNASSIGNED;
		return cardContainer.Suit;

	}

	public void PrintCards()
	{
		if (_leadPlayer != Player.UNDEFINED)
			GD.Print("Lead Player: " + PlayerToString[(int)_leadPlayer]);

		if (playerCard.Visible) GD.Print("Player: " + playerCard.ToString());
		else GD.Print("Player hasn't played");

		if (rightCard.Visible) GD.Print("Right: " + rightCard.ToString());
		else GD.Print("Right hasn't played");

		if (partnerCard.Visible) GD.Print("Partner: " + partnerCard.ToString());
		else GD.Print("Partner hasn't played");

		if (leftCard.Visible) GD.Print("Left: " + leftCard.ToString());
		else GD.Print("Left hasn't played");
	}
}
