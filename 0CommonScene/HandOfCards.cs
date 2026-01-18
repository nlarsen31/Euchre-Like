using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Xml;
using Godot;
using static GlobalProperties;

public partial class HandOfCards : Node2D
{
	// exports

	[Export]
	private PackedScene CardContainer;

	private const int MAX_HAND_SIZE = 13;

	private List<CardContainer> _CardsInHand;
	private List<CardContainer> _ConnectedCards;

	public int NumberOfCardsLeftToPlay
	{
		get
		{
			int numberOfVisibleCards = 0;
			foreach (CardContainer card in _CardsInHand)
				if (card.Visible) numberOfVisibleCards++;
			return numberOfVisibleCards;
		}
	}

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		_CardsInHand = new List<CardContainer>();
		_ConnectedCards = new List<CardContainer>();
	}

	public void addRandomHand()
	{
		if (Debugging) GD.Print("[Enter] addRandomHand");
		clearCards();
		Random random = new Random();
		for (int i = 0; i < MAX_HAND_SIZE; i++)
		{
			int rank = random.Next(0, 13);
			int suit = random.Next(0, 4);
			addCard((Rank)rank, (Suit)suit);
		}
		DrawHand();
		if (Debugging) GD.Print("[Leave] addRandomHand");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void addCard(Rank rank, Suit suit)
	{
		CardContainer card = (CardContainer)CardContainer.Instantiate();
		card.Suit = suit;
		card.Rank = rank;
		card.Visible = true;
		AddChild(card);
		_CardsInHand.Add(card);
		DrawHand();
	}

	public void clearCards()
	{
		foreach (CardContainer card in _CardsInHand)
		{
			if (card != null)
			{
				card.Visible = false;
				card.QueueFree();
			}
		}
		_CardsInHand.Clear();
	}

	public void DrawHand()
	{
		if (Debugging) GD.Print("[Enter] DrawHand");
		_CardsInHand.Sort();
		int numberOfVisibleCards = NumberOfCardsLeftToPlay;

		// Print each element for debugging
		if (Debugging)
		{
			GD.Print("Current Hand:");
			foreach (CardContainer card in _CardsInHand)
				GD.Print(card.ToString());
		}

		// Add half Width if we are an odd number of cards.

		int xStart;
		if (numberOfVisibleCards % 2 == 0)
			xStart = (numberOfVisibleCards - 1) * CARD_WIDTH / 2;
		else
			xStart = (numberOfVisibleCards / 2) * CARD_WIDTH;
		xStart *= -1;
		Vector2 pos = new Vector2(xStart, 0);

		foreach (CardContainer card in _CardsInHand)
		{
			if (card.Visible)
			{
				card.SetAnimation();
				card.Position = pos;
				pos.X += CARD_WIDTH;
			}
		}
		if (Debugging) GD.Print("[Leave] DrawHand");
	}

	public int NumberOfCardsInHand()
	{
		return _CardsInHand.Count;
	}
	public List<string> ExportHand()
	{
		List<string> list = new List<string>();

		foreach (CardContainer cardContainer in _CardsInHand)
		{
			list.Add(cardContainer.ToString());
		}

		return list;
	}

	// Rules for selecting upgrades
	// 1. No Card is allowed to be Jack and Trump wild
	public void ConnectVisibleCards(Callable method, UpgradeType upgradeType)
	{
		if (upgradeType == UpgradeType.NoJackToTrump)
		{
			// Skip allowing any Jacks to be selected for upgrade
			foreach (CardContainer cardContainer in _CardsInHand)
			{
				if (cardContainer.Visible && cardContainer.Rank != Rank.jack)
				{
					cardContainer.Connect("CardSelected", method);
					cardContainer.Selectable = true;
					_ConnectedCards.Add(cardContainer);
				}
			}
		}
		else if (upgradeType == UpgradeType.Strength)
		{
			// Do not allow 10 of trump to be selected for strength
			foreach (CardContainer cardContainer in _CardsInHand)
			{
				if (cardContainer.Visible && !(cardContainer.Rank == Rank.ten && cardContainer.Suit == CurrentTrump))
				{
					cardContainer.Connect("CardSelected", method);
					cardContainer.Selectable = true;
					_ConnectedCards.Add(cardContainer);
				}
			}
		}
		else // Connect all visible cards
		{
			foreach (CardContainer cardContainer in _CardsInHand)
			{
				if (cardContainer.Visible)
				{
					cardContainer.Connect("CardSelected", method);
					cardContainer.Selectable = true;
					_ConnectedCards.Add(cardContainer);
				}
			}
		}
	}

	public void ConnectVisibleCards(Callable method, Suit iLead)
	{
		bool voidInSuit = true;
		foreach (CardContainer cardContainer in _CardsInHand)
		{
			if (cardContainer.Visible)
			{
				if (cardContainer.Suit == iLead || iLead == Suit.UNASSIGNED)
				{
					voidInSuit = false;
					cardContainer.Connect("CardSelected", method);
					cardContainer.Selectable = true;
					_ConnectedCards.Add(cardContainer);
				}
			}
		}

		if (voidInSuit)
		{
			foreach (CardContainer cardContainer in _CardsInHand)
				if (cardContainer.Visible)
				{
					cardContainer.Connect("CardSelected", method);
					cardContainer.Selectable = true;
					_ConnectedCards.Add(cardContainer);
				}
		}
	}

	public void DisconnectVisibleCards(Callable method)
	{
		while (_ConnectedCards.Count > 0)
		{
			CardContainer card = _ConnectedCards[0];
			card.Disconnect("CardSelected", method);
			card.Selectable = false;
			_ConnectedCards.Remove(card);
		}
	}

	public void HideCard(string card)
	{
		foreach (CardContainer cardContainer in _CardsInHand)
		{
			if (card == cardContainer.ToString() && cardContainer.Visible)
			{
				GD.Print($"Hiding {card}");
				cardContainer.Visible = false;
				break;
			}
		}
		DrawHand();
	}

	public CardContainer GetCardContainer(string card)
	{
		foreach (CardContainer cardContainer in _CardsInHand)
		{
			if (card == cardContainer.ToString())
				return cardContainer;
		}
		return null;
	}
}