using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Xml;
using static GlobalProperties;

public partial class HandOfCards : Node2D
{
	// exports
	[Export]
	private PackedScene CardContainer;

	private const int MAX_HAND_SIZE = 18;

	private List<CardContainer> _CardsInHand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_CardsInHand = new List<CardContainer>();
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

	public void DrawHand()
	{
		GD.Print("[Enter] DrawHand");
		_CardsInHand.Sort();
		int numberOfVisibleCards = 0;
		foreach (CardContainer card in _CardsInHand)
			if (card.Visible) numberOfVisibleCards++;

		// Add halfWidth if we are an odd number of cards.
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


		GD.Print("[Enter] DrawHand");
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

	public void ConnectVisibleCards(Callable method)
	{
		foreach (CardContainer cardContainer in _CardsInHand)
			if (cardContainer.Visible)
			{
				cardContainer.Connect("CardSelected", method);
				cardContainer.Selectable = true;
			}
	}

	public void DisconnectVisibleCards(Callable method)
	{
		foreach (CardContainer cardContainer in _CardsInHand)
		{
			if (cardContainer.Visible)
				cardContainer.Disconnect("CardSelected", method);
			cardContainer.Selectable = false;
		}
	}


	public void HideCard(string card)
	{
		foreach (CardContainer cardContainer in _CardsInHand)
		{
			if (card == cardContainer.ToString())
				cardContainer.Visible = false;
		}
		DrawHand();
	}
}