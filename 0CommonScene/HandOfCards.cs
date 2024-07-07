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

	private List<CardContainer> CardsInHand;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CardsInHand = new List<CardContainer>();
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
		CardsInHand.Add(card);
		DrawHand();
	}

	public void DrawHand()
	{
		CardsInHand.Sort();
		int count = 0;
		foreach (CardContainer card in CardsInHand)
			if (card.Visible) count++;

		if (count % 2 == 0)
		{
			Vector2 pos = new Vector2((-1) * CARD_WIDTH / 2, 0);
			// Draw the hand going left
			int idx = count / 2 - 1;
			while (idx >= 0)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X -= CARD_WIDTH;
				idx--;
			}
			// Draw the hand going right
			idx = count / 2;
			pos = new Vector2(CARD_WIDTH / 2, 0);
			while (idx < count)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X += CARD_WIDTH;
				idx++;
			}
		}
		else
		{
			// Draw the middle card
			int idx = count / 2;
			Vector2 pos = new Vector2(0, 0);
			CardsInHand[idx].Position = pos;
			CardsInHand[idx].Visible = true;
			CardsInHand[idx].SetAnimation();

			// Draw the hand going left
			pos = new Vector2((-1) * CARD_WIDTH, 0);
			idx = count / 2 - 1;
			while (idx >= 0)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X -= CARD_WIDTH;
				idx--;
			}
			// Draw the hand going right
			idx = count / 2 + 1;
			pos = new Vector2(CARD_WIDTH, 0);
			while (idx < count)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X += CARD_WIDTH;
				idx++;
			}
		}
	}

	public int NumberOfCardsInHand()
	{
		return CardsInHand.Count;
	}
	public List<string> ExportHand()
	{
		List<string> list = new List<string>();

		foreach (CardContainer cardContainer in CardsInHand)
		{
			list.Add(cardContainer.ToString());
		}

		return list;
	}

	public void ConnectVisibleCards(Callable method)
	{
		foreach (CardContainer cardContainer in CardsInHand)
			if (cardContainer.Visible)
			{
				cardContainer.Connect("CardSelected", method);
				cardContainer.Selectable = true;
			}
	}

	public void DisconnectVisibleCards(Callable method)
	{
		foreach (CardContainer cardContainer in CardsInHand)
		{
			if (cardContainer.Visible)
				cardContainer.Disconnect("CardSelected", method);
			cardContainer.Selectable = false;
		}
	}


	public void HideCard(string card)
	{
		foreach (CardContainer cardContainer in CardsInHand)
		{
			if (card == cardContainer.ToString())
				cardContainer.Visible = false;
		}
		DrawHand();
	}
}