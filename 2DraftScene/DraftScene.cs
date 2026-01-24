using System;
using System.Collections.Generic;
using Godot;
using static GlobalMethods;
using static GlobalProperties;
public partial class DraftScene : Node2D
{
	[Export]
	private PackedScene CardContainer;

	int currUnselectedIdx = 0;
	List<CardContainer> UnselectedCards;

	private Dictionary<Rank, double> RankChances = new Dictionary<Rank, double>
	{
		{Rank.ace, 1.0/13.0},
		{Rank.two, 1.0/13.0},
		{Rank.three, 1.0/13.0},
		{Rank.four, 1.0/13.0},
		{Rank.five, 1.0/13.0},
		{Rank.six, 1.0/13.0},
		{Rank.seven, 1.0/13.0},
		{Rank.eight, 1.0/13.0},
		{Rank.nine, 1.0/13.0},
		{Rank.ten, 1.0/13.0},
		{Rank.jack, 1.0/13.0},
		{Rank.queen, 1.0/13.0},
		{Rank.king, 1.0/13.0}
	};
	const double halfChance = 1.0 / 16.0 / 4.0; // remove this much chance everytime a rank is picked
	Rank[] ranks = GetAllRanks();

	public enum DraftingMode
	{
		FULLY_RANDOM,
		RANK_BASED
	}
	public DraftingMode CurrentDraftingMode = DraftingMode.RANK_BASED;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UnselectedCards = new List<CardContainer>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits())
			{
				CardContainer card = (CardContainer)CardContainer.Instantiate();
				card.Suit = suit;
				card.Rank = rank;
				UnselectedCards.Add(card);
				card.Visible = false;
			}
		}
		RandomizeList<CardContainer>(UnselectedCards);
		GlobalGamePhase = Phase.drafting;
		DisplayNextCards(4);
	}

	public Rank GetRandomRank()
	{
		// Get a number between 0 and 1
		double random = randy.NextDouble();
		double cumulative = 0.0;

		Rank chosenRank = Rank.undefined;
		// Iterate through Rank Chances
		foreach (Rank rank in ranks)
		{
			// Once we have hit the rank we want use it
			cumulative += RankChances[rank];
			if (random < cumulative)
			{
				chosenRank = rank;
				break;
			}
		}

		// This needs to be moved to after the card is picked
		// // Half the chance of getting the same rank again
		// if (chosenRank != Rank.undefined)
		// {
		// 	double newChance = RankChances[chosenRank] / 2.0;
		// 	double delaForOtherCards = newChance / (RankChances.Count - 1);
		// 	foreach (Rank rank in ranks)
		// 	{
		// 		if (rank == chosenRank)
		// 		{
		// 			RankChances[rank] = newChance;
		// 		}
		// 		else
		// 		{
		// 			RankChances[rank] += delaForOtherCards;
		// 		}
		// 	}
		// }
		return chosenRank;
	}

	public void DecreaseRankChance(Rank rank)
	{
		// Half the chance of getting the same rank again
		if (rank != Rank.undefined)
		{
			double newChance = RankChances[rank] - halfChance;
			if (newChance < 0.001)
			{
				newChance = 0.0;
			}
			double deltaForOtherCards = halfChance / (RankChances.Count - 1);
			foreach (Rank r in ranks)
			{
				if (r == rank)
				{
					RankChances[r] = newChance;
				}
				else
				{
					RankChances[r] += deltaForOtherCards;
				}
			}
		}
	}

	public void DisplayNextCards(int count)
	{
		if (CurrentDraftingMode == DraftingMode.FULLY_RANDOM)
		{
			// Add first 4 to the CardSelection
			CardSelection cardSelection = GetNode<CardSelection>("CardSelection");
			cardSelection.clearCards();
			for (int i = currUnselectedIdx; i < currUnselectedIdx + count; i++)
			{
				CardContainer card = UnselectedCards[i];
				cardSelection.AddCard(UnselectedCards[i]);
			}
			cardSelection.DrawCards();
			Godot.Callable callable = new Callable(this, "SelectCardCallback");
			cardSelection.ConnectCards(callable);
			currUnselectedIdx += count;
		}
		else if (CurrentDraftingMode == DraftingMode.RANK_BASED)
		{
			// Add first 4 to the CardSelection
			CardSelection cardSelection = GetNode<CardSelection>("CardSelection");
			cardSelection.clearCards();
			Random rand = new Random();
			for (int i = 0; i < count; i++)
			{
				// Pick a random rank based on Rank Chances
				Rank rank = GetRandomRank();

			}
			cardSelection.DrawCards();
			Godot.Callable callable = new Callable(this, "SelectCardCallback");
			cardSelection.ConnectCards(callable);
		}
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
		Tuple<Rank, Suit> tuple = GetSuitRankFromString(card);
		// GD.Print(tuple);
		hand.addCard(tuple.Item1, tuple.Item2);

		// Get 4 new cards.
		if (hand.NumberOfCardsInHand() < HAND_SIZE)
		{
			DisplayNextCards(4);
		}
		else
		{
			CurrentHand = hand.ExportHand();
			GetTree().ChangeSceneToFile("res://3PlayingScene/Playing.tscn");
		}
	}
}
