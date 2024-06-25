using Godot;
using System;
using System.Collections.Generic;

using static GlobalProperties;
using static GlobalMethods;
public partial class DraftScene : Node2D
{
	[Export]
	private PackedScene CardContainer;
	
	int currUnselectedIdx = 0;
	List<CardContainer> UnselectedCards;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UnselectedCards = new List<CardContainer>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits())
			{
				CardContainer card = (CardContainer)CardContainer.Instantiate();
				card.SetSuit(suit);
				card.SetRank(rank);
				UnselectedCards.Add(card);
				card.Visible = false;
			}
		}
		RandomizeList<CardContainer>(UnselectedCards);
		GlobalGamePhase = Phase.drafting;
		DisplayNextCards(4);
	}

	public void DisplayNextCards(int count)
	{
		// Add first 4 to the CardSelection
		CardSelection cardSelection = GetNode<CardSelection>("CardSelection");
		cardSelection.clearCards();
		for (int i = currUnselectedIdx; i < currUnselectedIdx+count; i++)
		{
			CardContainer card = UnselectedCards[i];
			cardSelection.AddCard(UnselectedCards[i]);
		}
		cardSelection.DrawCards();
		Godot.Callable callable = new Callable(this, "SelectCardCallback");
		cardSelection.ConnectCards(callable);
		currUnselectedIdx += count;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SelectCardCallback(string card)
	{
		// Disconnect all cards
		CardSelection cardSelection = GetNode<CardSelection>("CardSelection");
		Godot.Callable callable = new Callable(this, "SelectCardCallback");
		cardSelection.DisconnectCards(callable);

		// Add to picked cards
		// GD.Print("SelectCardCallback()" + card);
		HandOfCards hand = GetNode<HandOfCards>("HandOfCards");
		Tuple<Rank, Suit> tuple =  GetSuitRankFromString(card);
		GD.Print(tuple);
		hand.addCard(tuple.Item1, tuple.Item2);
		
		// Get 4 new cards.
		DisplayNextCards(4);
	}
}
