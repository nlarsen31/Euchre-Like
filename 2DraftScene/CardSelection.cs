using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using static GlobalProperties;

public partial class CardSelection : Node2D
{
	private List<CardContainer> SelectableCards;
	// Called when the node enters the scene tree for the first time.

	                                                                                                                                                                                
	public override void _Ready()
	{
		SelectableCards = new List<CardContainer>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetLeftLeft(CardContainer card)
	{
		Vector2 LeftLeftV = GetNode<CardContainer>("LeftLeft").Position;
		AddChild(card);
		card.Visible = true;
		card.Position = LeftLeftV;
	}
	public void AddCard(CardContainer card) {
		GD.Print(SuitToString[(int)card.Suit]);
		SelectableCards.Add(card);
	}
	public void clearCards()
	{
		SelectableCards.Clear();
	}

	public void  DrawCards()
	{
		CardContainer LeftLeft =  GetNode<CardContainer>("LeftLeft");
		CardContainer LeftRight =  GetNode<CardContainer>("LeftRight");
		CardContainer RightLeft =  GetNode<CardContainer>("RightLeft");
		CardContainer RightRight =  GetNode<CardContainer>("RightRight");

		LeftLeft.SetAnimation(SelectableCards[0].Rank, SelectableCards[0].Suit);
		LeftRight.SetAnimation(SelectableCards[1].Rank, SelectableCards[1].Suit);
		RightLeft.SetAnimation(SelectableCards[2].Rank, SelectableCards[2].Suit);
		RightRight.SetAnimation(SelectableCards[3].Rank, SelectableCards[3].Suit);
		RightRight.FlipDown();

		LeftLeft.Visible = true;
		LeftRight.Visible = true;
		RightLeft.Visible = true;
		RightRight.Visible = true;
	}

	public void ConnectCards(Callable method)
	{
		CardContainer LeftLeft =  GetNode<CardContainer>("LeftLeft");
		CardContainer LeftRight =  GetNode<CardContainer>("LeftRight");
		CardContainer RightLeft =  GetNode<CardContainer>("RightLeft");
		CardContainer RightRight =  GetNode<CardContainer>("RightRight");

		LeftLeft.Connect("CardSelected", method);
		LeftRight.Connect("CardSelected", method);
		RightLeft.Connect("CardSelected", method);
		RightRight.Connect("CardSelected", method);
	}
	public void DisconnectCards(Callable method)
	{
		CardContainer LeftLeft =  GetNode<CardContainer>("LeftLeft");
		CardContainer LeftRight =  GetNode<CardContainer>("LeftRight");
		CardContainer RightLeft =  GetNode<CardContainer>("RightLeft");
		CardContainer RightRight =  GetNode<CardContainer>("RightRight");

		LeftLeft.Disconnect("CardSelected", method);
		LeftRight.Disconnect("CardSelected", method);
		RightLeft.Disconnect("CardSelected", method);
		RightRight.Disconnect("CardSelected", method);
	}
}
