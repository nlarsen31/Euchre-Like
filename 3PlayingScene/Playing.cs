using Godot;
using System;

using static GlobalProperties;
using static GlobalMethods;
using System.Collections.Generic;

public partial class Playing : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Make a set with all the cards
		HashSet<string> Deck = new HashSet<string>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits()) {
				string s  = SuitToString[(int)suit] + "_" + RankToString[(int)rank];
				Deck.Add(s);
			}
		}

		HandOfCards Hand = GetNode<HandOfCards>("HandOfCards");
		foreach(string s in CurrentHand)
		{
			// GD.Print(s);
			Tuple<Rank, Suit> tup = GetSuitRankFromString(s);
			Deck.Remove(s);
			Hand.addCard(tup.Item1, tup.Item2);
		}

		GD.Print("Cards in hand:");
		foreach(string s in CurrentHand) {
			GD.Print(s);
		}
		GD.Print("\n\nCards left in deck:");
		foreach(string s in Deck) {
			GD.Print(s);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
