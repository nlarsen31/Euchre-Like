namespace Eurchelike;

using Godot;
using System;

using static GlobalProperties;
using static GlobalMethods;
using static PlayerAgents;
using System.Collections.Generic;
using System.Linq;

public partial class Playing : Node2D
{
	// Scene Phases
	public Player ActivePlayer = Player.RIGHT;
	private PlayedCards _PlayedCards;
	private HandOfCards _HandOfCards;
	int TrickCount = 0;
	public CardContainer[] PlayedCards = new CardContainer[4];

	[Export]
	private PackedScene CardContainer;

	private List<CardContainer> LeftHand = new List<CardContainer>();
	private List<CardContainer> RightHand = new List<CardContainer>();
	private List<CardContainer> PartnerHand = new List<CardContainer>();
	private Timer PlayTimer;
	Callable _Callable;

	// Look at Trump ActivePlayer PlayedCards, assume that ActivePlayer+1 was the player that lead.
	// TODO: Refactor to be a memeber of playedCards...
	public Player HandEval()
	{
		if (_PlayedCards.HaveAllPlayersPlayed)
		{
			Player LeadPlayer = NextPlayer(ActivePlayer);
			Player winner = LeadPlayer;
			_PlayedCards.SetPlayedCards(PlayedCards);
			Suit leadSuit = PlayedCards[(int)NextPlayer(ActivePlayer)].Suit;

			Player CurrentPlayer = NextPlayer(LeadPlayer);
			for (int i = 0; i < 3; i++)
			{
				CardContainer winnerCard = PlayedCards[(int)winner];
				CardContainer currentCard = PlayedCards[(int)CurrentPlayer];

				if (currentCard.Suit == leadSuit || currentCard.Suit == Trump)
				{
					if (currentCard > winnerCard)
						winner = CurrentPlayer;
				}
				CurrentPlayer = NextPlayer(CurrentPlayer);
			}

			return winner;
		}
		return Player.UNDEFINED;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayTimer = GetNode<Timer>("%Timer");
		_PlayedCards = GetNode<PlayedCards>("PlayedCards");
		_HandOfCards = GetNode<HandOfCards>("HandOfCards");
		_Callable = new Callable(this, "SelectCardCallback");

		// Make a set with all the cards
		HashSet<string> Deck = new HashSet<string>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits())
			{
				string s = SuitToString[(int)suit] + "_" + RankToString[(int)rank];
				Deck.Add(s);
			}
		}

		foreach (string s in CurrentHand)
		{
			// GD.Print(s);
			Tuple<Rank, Suit> tup = GetSuitRankFromString(s);
			Deck.Remove(s);
			_HandOfCards.addCard(tup.Item1, tup.Item2);
		}

		// Deal cards randomly to each of the 3 players left
		List<string> cardsLeft = Deck.ToList<string>();
		RandomizeList<string>(cardsLeft);
		// Cards 0-12 are left, 13-25 partner, 26-38 are Right
		LeftHand.Clear();
		for (int i = 0; i < 13; i++)
		{
			Tuple<Rank, Suit> tup = GetSuitRankFromString(cardsLeft[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			LeftHand.Add(card);
		}
		PartnerHand.Clear();
		for (int i = 13; i < 26; i++)
		{
			Tuple<Rank, Suit> tup = GetSuitRankFromString(cardsLeft[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			PartnerHand.Add(card);
		}
		RightHand.Clear();
		for (int i = 26; i < 39; i++)
		{
			Tuple<Rank, Suit> tup = GetSuitRankFromString(cardsLeft[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			RightHand.Add(card);
		}
		SetHandCountLabels();

		// randomly select trump
		Chip TrumpChip = GetNode<Chip>("TrumpChip");
		Trump = (Suit)randy.Next(0, 4);
		TrumpChip.SetAnimation(Trump);

		// Select random player to lead
		ActivePlayer = (Player)randy.Next(0, 4);
		ActivePlayer = Player.PLAYER; // Force player to beth active player. for now
		Chip LeadChip = GetNode<Chip>("LeadChip");
		LeadChip.SetLeadPosition(ActivePlayer);

		PlayTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetHandCountLabels()
	{
		string LeftLabelStr = $"Left has {LeftHand.Count} cards left";
		string RightLabelStr = $"Right has {RightHand.Count} cards left";
		string PartnerLabelStr = $"Partner has {PartnerHand.Count} cards left";
		Label LeftLabel = GetNode<Label>("LeftLabel");
		Label RightLabel = GetNode<Label>("RightLabel");
		Label PartnerLabel = GetNode<Label>("PartnerLabel");

		LeftLabel.Text = LeftHand.Count.ToString();
		RightLabel.Text = RightHand.Count.ToString();
		PartnerLabel.Text = PartnerHand.Count.ToString();

		LeftLabel.TooltipText = LeftLabelStr;
		RightLabel.TooltipText = RightLabelStr;
		PartnerLabel.TooltipText = PartnerLabelStr;

	}


	// Play Turn Functions
	public void PlayTurn()
	{
		PlayTimer.Stop();
		if (_PlayedCards.HaveAllPlayersPlayed)
		{
			Player winner = HandEval();
			GD.Print("winner is " + PlayerToString[(int)winner]);
		}
		else
		{
			if (ActivePlayer == Player.PLAYER)
			{
				PlayerTurn();
				ActivePlayer = NextPlayer(ActivePlayer);
			}
			else if (ActivePlayer == Player.LEFT)
			{
				CardContainer choice = LeftTurn(LeftHand);
				_PlayedCards.ShowAndSetCard(choice.ToString(), Player.LEFT);
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
			else if (ActivePlayer == Player.RIGHT)
			{
				CardContainer choice = RightTurn(RightHand);
				_PlayedCards.ShowAndSetCard(choice.ToString(), Player.RIGHT);
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
			else if (ActivePlayer == Player.PARTNER)
			{
				CardContainer choice = PartnerTurn(PartnerHand);
				_PlayedCards.ShowAndSetCard(choice.ToString(), Player.PARTNER);
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
		}
	}

	// Each PLayer Functions:
	public void PlayerTurn()
	{
		_HandOfCards.ConnectVisibleCards(_Callable);
	}

	public void SelectCardCallback(string card)
	{
		_HandOfCards.DisconnectVisibleCards(_Callable);
		_PlayedCards.ShowAndSetCard(card, Player.PLAYER);

		_HandOfCards.HideCard(card);

		PlayTimer.Start();
	}



	// Connected functions.
	public void OnTimeout()
	{
		PlayTimer.Stop();
		PlayTurn();
	}
}
