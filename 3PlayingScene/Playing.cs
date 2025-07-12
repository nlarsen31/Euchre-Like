namespace Eurchelike;

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static GlobalMethods;
using static GlobalProperties;
using static PlayerAgents;

public partial class Playing : Node2D
{
	const int STARTING_REQUIRED_TRICKS = 3;
	// Scene Phases

	public Player ActivePlayer = Player.RIGHT;
	private PlayedCards _PlayedCards;
	private HandOfCards _HandOfCards;
	int TrickCount = 0;
	public CardContainer[] PlayedCardsArr = new CardContainer[4];

	[Export]
	private PackedScene CardContainer;

	private List<CardContainer> LeftHand = new List<CardContainer>();
	private List<CardContainer> RightHand = new List<CardContainer>();
	private List<CardContainer> PartnerHand = new List<CardContainer>();
	private Timer PlayTimer;
	private Timer _PlayFinishedTimer;
	private Timer _HandFinishedTimer;
	Callable _Callable;

	private ScoreBoard _ScoreBoard;

	// Look at Trump ActivePlayer PlayedCards, assume that ActivePlayer+1 was the player that lead.
	// TODO: Refactor to be a memeber of playedCards...

	public Player HandEval()
	{
		Player winner = Player.UNDEFINED;
		if (Debugging)
		{
			GD.Print("[Enter] HandEval()");
			GD.Print("LeadSuit: " + SuitToString[(int)_PlayedCards.GetLeadSuit()]);
			GD.Print("Trump: " + SuitToString[(int)Trump]);
			_PlayedCards.PrintCards();
		}
		if (_PlayedCards.HaveAllPlayersPlayed)
		{
			Player LeadPlayer = _PlayedCards.GetLeadPlayer();
			winner = LeadPlayer;
			_PlayedCards.SetPlayedCards(PlayedCardsArr);
			Suit leadSuit = _PlayedCards.GetLeadSuit();

			Player CurrentPlayer = NextPlayer(LeadPlayer);
			for (int i = 0; i < 3; i++)
			{
				CardContainer winnerCard = PlayedCardsArr[(int)winner];
				CardContainer currentCard = PlayedCardsArr[(int)CurrentPlayer];

				if (currentCard.Suit == leadSuit || currentCard.Suit == Trump)
				{
					if (currentCard > winnerCard)
						winner = CurrentPlayer;
				}
				CurrentPlayer = NextPlayer(CurrentPlayer);
			}
		}
		if (Debugging)
		{
			GD.Print("Winner is: " + PlayerToString[(int)winner]);
			GD.Print("[Leave] HandEval()");
		}
		return winner;
	}

	// Set up Player hands
	public void SetupPlayersHands()
	{
		// Make a set with all the cards
		_HandOfCards.clearCards();

		HashSet<string> Deck = new HashSet<string>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits())
			{
				string s = SuitToString[(int)suit] + "_" + RankToString[(int)rank];
				Deck.Add(s);
			}
		}

		if (CurrentHand == null)
		{
			CurrentHand = new List<string>();
			foreach (string card in Deck)
			{
				CurrentHand.Add(card);
				if (CurrentHand.Count == 13) break;
			}
		}

		// Remove all of players cards

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
		ActivePlayer = Player.PLAYER;
		Chip LeadChip = GetNode<Chip>("LeadChip");
		LeadChip.SetLeadPosition(ActivePlayer);
		_HandOfCards.DrawHand();
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayTimer = GetNode<Timer>("%Timer");
		_PlayFinishedTimer = GetNode<Timer>("PlayFinishedTimer");
		_HandFinishedTimer = GetNode<Timer>("HandFinishedTimer");
		_PlayedCards = GetNode<PlayedCards>("PlayedCards");
		_HandOfCards = GetNode<HandOfCards>("HandOfCards");
		_Callable = new Callable(this, "SelectCardCallback");
		_ScoreBoard = GetNode<ScoreBoard>("ScoreBoard");

		SetupPlayersHands();
		// Reset scoreboad
		_ScoreBoard.Reset(STARTING_REQUIRED_TRICKS);

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

	// Check if player has won the required number of tricks.

	public bool CheckTrickRequirement()
	{
		if (_ScoreBoard.TricksWon >= _ScoreBoard.TricksRequired)
		{
			return true;
		}
		return false;
	}

	// Play Turn Functions

	public void PlayTurn()
	{
		Label resultLabel = GetNode<Label>("ResultLabel");
		resultLabel.Visible = false;
		_PlayedCards.Visible = true;
		PlayTimer.Stop();
		if (_PlayedCards.HaveAllPlayersPlayed)
		{
			Player winner = HandEval();
			GD.Print("winner is " + PlayerToString[(int)winner]);
			if (winner == Player.PLAYER || winner == Player.PARTNER)
			{
				_ScoreBoard.TricksWon += 1;
			}
			_ScoreBoard.TricksLeft -= 1;

			if (CheckTrickRequirement())
			{
				resultLabel.Text = $"You won {_ScoreBoard.TricksWon} Tricks!";
				resultLabel.Visible = true;
				_PlayedCards.Visible = false;
				SetupPlayersHands();
				_ScoreBoard.Reset(_ScoreBoard.TricksRequired + 2);
				_PlayFinishedTimer.Start();
			}
			else
			{
				ActivePlayer = winner;
				resultLabel.Visible = true;
				resultLabel.Text = $"{PlayerToString[(int)winner]} won the trick!";
				_HandFinishedTimer.Start();
			}

		}
		else
		{
			Suit leadSuit = _PlayedCards.GetLeadSuit();
			if (ActivePlayer == Player.PLAYER)
			{
				PlayerTurn();
				ActivePlayer = NextPlayer(ActivePlayer);
			}
			else if (ActivePlayer == Player.LEFT)
			{
				CardContainer choice = LeftTurn(LeftHand, leadSuit);
				LeftHand.Remove(choice);
				PlayCard(Player.LEFT, choice.ToString());
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
			else if (ActivePlayer == Player.RIGHT)
			{
				CardContainer choice = RightTurn(RightHand, leadSuit);
				RightHand.Remove(choice);
				PlayCard(Player.RIGHT, choice.ToString());
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
			else if (ActivePlayer == Player.PARTNER)
			{
				CardContainer choice = PartnerTurn(PartnerHand, leadSuit);
				PartnerHand.Remove(choice);
				PlayCard(Player.PARTNER, choice.ToString());
				ActivePlayer = NextPlayer(ActivePlayer);
				PlayTimer.Start();
			}
		}
	}

	public void PlayCard(Player iPlayer, string card)
	{
		_PlayedCards.ShowAndSetCard(card, iPlayer);
		// PlayCard(Player.PLAYER, card);
	}

	// Each PLayer Functions:

	public void PlayerTurn()
	{
		_HandOfCards.ConnectVisibleCards(_Callable, _PlayedCards.GetLeadSuit());
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

		if (RightHand.Count == 0 && LeftHand.Count == 0 &&
			PartnerHand.Count == 0 && _HandOfCards.NumberOfCardsLeftToPlay == 0)
		{
			Label resultLabel = GetNode<Label>("ResultLabel");
			resultLabel.Text = $"You won {_ScoreBoard.TricksWon} Tricks!";
			resultLabel.Visible = true;
			_PlayedCards.Visible = false;
		}
		else PlayTurn();
	}

	public void OnPlayFinishedTimeout()
	{
		_PlayFinishedTimer.Stop();
		CurrentHand = _HandOfCards.ExportHand();
		GetTree().ChangeSceneToFile("res://4UpgradeScene/Upgrade.tscn");
	}

	public void OnHandFinishedTimeout()
	{
		Label resultLabel = GetNode<Label>("ResultLabel");
		_HandFinishedTimer.Stop();
		_PlayedCards.ClearCards();
		resultLabel.Visible = false;
		PlayTimer.Start();
	}
}
