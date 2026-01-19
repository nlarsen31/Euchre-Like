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
	// TODO: Refactor to be a member of playedCards...

	public Player HandEval()
	{
		Player winner = Player.UNDEFINED;
		if (Debugging)
		{
			GD.Print("[Enter] HandEval()");
			GD.Print("LeadSuit: " + SuitToString[(int)_PlayedCards.GetLeadSuit()]);
			GD.Print("Trump: " + SuitToString[(int)CurrentTrump]);
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

				if (currentCard.Suit == leadSuit || currentCard.Suit == CurrentTrump)
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

	// Set up Player hands, if they are not already set up.
	public void SetupPlayersHands()
	{
		GD.Print("SetupPlayersHand ");
		// 1. Assign NonPlayerCards if not already done.
		if (NonPlayerCards.Count == 0 || true)
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

			NonPlayerCards = Deck.ToList<string>();
		}
		else
		{
			_HandOfCards.clearCards();
			// Set up only players hand
			foreach (string s in CurrentHand)
			{
				if (Debugging) GD.Print(s);

				Tuple<Rank, Suit> tup = GetSuitRankFromString(s);
				_HandOfCards.addCard(tup.Item1, tup.Item2);
			}
		}
		// 2. Randomize the order of NonPlayerCards
		RandomizeList<string>(NonPlayerCards);

		// 3. Deal out cards based on index.
		// Cards 0-12 are left, 13-25 partner, 26-38 are Right
		LeftHand.Clear();
		for (int i = 0; i < 13; i++)
		{
			if (Debugging) GD.Print("Adding Left" + NonPlayerCards[i]);
			Tuple<Rank, Suit> tup = GetSuitRankFromString(NonPlayerCards[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			LeftHand.Add(card);
		}
		PartnerHand.Clear();
		for (int i = 13; i < 26; i++)
		{
			if (Debugging) GD.Print("Adding Partner" + NonPlayerCards[i]);
			Tuple<Rank, Suit> tup = GetSuitRankFromString(NonPlayerCards[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			PartnerHand.Add(card);
		}
		RightHand.Clear();
		for (int i = 26; i < 39; i++)
		{
			if (Debugging) GD.Print("Adding Right" + NonPlayerCards[i]);
			Tuple<Rank, Suit> tup = GetSuitRankFromString(NonPlayerCards[i]);
			CardContainer card = (CardContainer)CardContainer.Instantiate();
			card.Rank = tup.Item1;
			card.Suit = tup.Item2;
			RightHand.Add(card);
		}
		SetHandCountLabels();

		// 4. randomly select CurrentTrump
		Chip TrumpChip = GetNode<Chip>("TrumpChip");
		if (CurrentTrump == Suit.UNASSIGNED)
			CurrentTrump = (Suit)randy.Next(0, 4);
		TrumpChip.SetAnimation(CurrentTrump);

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
		CurrentWonGameState = WonGameState.NotFinished;

		GD.Print("Playing_Ready " + NonPlayerCards.Count);
		SetupPlayersHands();

		// Reset scoreboard
		_ScoreBoard.Reset(RequiredTricks);

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
		GD.Print("[Enter] PlayTurn");
		Label resultLabel = GetNode<Label>("ResultLabel");
		resultLabel.Visible = false;
		_PlayedCards.Visible = true;
		PlayTimer.Stop();

		if (_PlayedCards.HaveAllPlayersPlayed || _ScoreBoard.TricksLeft == 0 || CheckTrickRequirement())
		{
			Player winner = HandEval();
			GD.Print("winner is " + PlayerToString[(int)winner]);
			if (winner == Player.PLAYER || winner == Player.PARTNER)
			{
				_ScoreBoard.TricksWon += 1;
			}
			_ScoreBoard.TricksLeft -= 1;

			GD.Print("Tricks won: " + _ScoreBoard.TricksWon);
			GD.Print("Tricks left: " + _ScoreBoard.TricksLeft);
			if (CheckTrickRequirement())
			{
				resultLabel.Text = $"You won {_ScoreBoard.TricksWon} Tricks!";
				resultLabel.Visible = true;
				_PlayedCards.Visible = false;
				SetupPlayersHands();
				_PlayFinishedTimer.Start();
			}
			else if (_ScoreBoard.TricksLeft == 0)
			{
				resultLabel.Text = $"You lost! Only {_ScoreBoard.TricksWon} Tricks!";
				resultLabel.Visible = true;
				_PlayedCards.Visible = false;
				SetupPlayersHands();
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
				GD.Print("Starting From Left PlayTimer");
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
		GD.Print("[Leave] PlayTurn");
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

		if ((RightHand.Count == 0 && LeftHand.Count == 0 &&
			PartnerHand.Count == 0 && _HandOfCards.NumberOfCardsLeftToPlay == 0)
			|| CheckTrickRequirement())
		{
			Label resultLabel = GetNode<Label>("ResultLabel");
			resultLabel.Visible = true;
			_PlayedCards.Visible = false;
			GD.Print("All hands are empty, finishing play");
			_PlayFinishedTimer.Start();
		}
		else PlayTurn();
	}

	public void OnPlayFinishedTimeout()
	{
		GD.Print("[Enter] OnPlayFinishedTimeout");
		_PlayFinishedTimer.Stop();

		// Check if player won the last hand. 
		// TODO: This needs to be refactored to have common logic for when this is added.
		Player winner = HandEval();
		if ((winner == Player.PLAYER || winner == Player.PARTNER) && _ScoreBoard.TricksLeft == 1)
		{
			_ScoreBoard.TricksWon += 1;
		}
		_ScoreBoard.TricksLeft -= 1;

		GD.Print("Tricks won: " + _ScoreBoard.TricksWon);
		GD.Print("Tricks left: " + _ScoreBoard.TricksLeft);
		GD.Print("_ScoreBoard.TricksRequired: " + _ScoreBoard.TricksRequired);
		bool won = _ScoreBoard.TricksWon >= _ScoreBoard.TricksRequired;
		GD.Print("Player won: " + won);

		if (won && _ScoreBoard.TricksRequired < 13)
		{
			GD.Print("Player won enough tricks to upgrade.");
			_ScoreBoard.Reset(_ScoreBoard.TricksRequired + 2);
			CurrentHand = _HandOfCards.ExportHand();
			CurrentWonGameState = WonGameState.NotFinished;
			GetTree().ChangeSceneToFile("res://4UpgradeScene/Upgrade.tscn");
		}
		else if (won)
		{
			GD.Print("Player won the game.");
			CurrentWonGameState = WonGameState.Won;
			GetTree().ChangeSceneToFile("res://5Results/Results.tscn");
		}
		else
		{
			GD.Print("Player lost the game.");
			CurrentWonGameState = WonGameState.Lost;
			GetTree().ChangeSceneToFile("res://5Results/Results.tscn");
		}
		GD.Print("[Leave] OnPlayFinishedTimeout");
	}

	public void OnHandFinishedTimeout()
	{
		GD.Print("[Enter] OnHandFinishedTimeout");
		Label resultLabel = GetNode<Label>("ResultLabel");
		_HandFinishedTimer.Stop();
		_PlayedCards.ClearCards();
		resultLabel.Visible = false;
		PlayTimer.Start();
		GD.Print("[Leave] OnHandFinishedTimeout");
	}
}
