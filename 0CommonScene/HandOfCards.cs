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

	List<CardContainer> CardsInHand;
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
		card.SetSuit(suit);
		card.SetRank(rank);
		card.Visible = false;
		AddChild(card);
		CardsInHand.Add(card);
		DrawHand();
	}

	public void DrawHand()
	{
		CardsInHand.Sort();

		// TODO: Remove once debugging is not needed
		foreach (CardContainer cd in CardsInHand) {
			GD.Print(cd);
		}

		if (CardsInHand.Count % 2 == 0)
		{
			Vector2 pos = new Vector2((-1)*CARD_WIDTH/2, 0);
			// Draw the hand going left
			int idx = CardsInHand.Count/2 - 1;
			while (idx >= 0) 
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X -= CARD_WIDTH;
				idx --;
			}
			// Draw the hand going right
			idx = CardsInHand.Count/2;
			pos = new Vector2(CARD_WIDTH/2, 0);
			while (idx < CardsInHand.Count)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X += CARD_WIDTH;
				idx ++;
			}
		}
		else
		{
			// Draw the middle card
			int idx = CardsInHand.Count/2;
			Vector2 pos = new Vector2(0, 0);
			CardsInHand[idx].Position = pos;
			CardsInHand[idx].Visible = true;
			CardsInHand[idx].SetAnimation();

			// Draw the hand going left
			pos = new Vector2((-1)*CARD_WIDTH, 0);
			idx = CardsInHand.Count/2 - 1;
			while (idx >= 0) 
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X -= CARD_WIDTH;
				idx --;
			}
			// Draw the hand going right
			idx = CardsInHand.Count/2 + 1;
			pos = new Vector2(CARD_WIDTH, 0);
			while (idx < CardsInHand.Count)
			{
				CardsInHand[idx].Position = pos;
				CardsInHand[idx].Visible = true;
				CardsInHand[idx].SetAnimation();
				pos.X += CARD_WIDTH;
				idx ++;
			}
		}
	}
}
