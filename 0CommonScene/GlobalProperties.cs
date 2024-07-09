using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public partial class GlobalProperties : Node
{
	public static bool Debugging = true;
	public enum Suit
	{
		SPADES,
		HEARTS,
		CLUBS,
		DIAMONDS,
		UNASSIGNED
	}
	public static string[] SuitToString = { "spades", "hearts", "clubs", "diamonds", "unf" };
	public enum Rank
	{
		two,
		three,
		four,
		five,
		six,
		seven,
		eight,
		nine,
		ten,
		jack,
		queen,
		king,
		ace,
		undefined
	}
	public static string[] RankToString = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "j", "q", "k", "a", "unf" };

	public enum Player
	{
		PLAYER,
		RIGHT,
		PARTNER,
		LEFT,
		UNDEFINED
	}
	public static string[] PlayerToString = {
		"player",
		"right",
		"partner",
		"left"
	};
	public Dictionary<Player, Player> NextPlayer = new Dictionary<Player, Player> {
		{Player.PLAYER, Player.RIGHT},
		{Player.RIGHT, Player.PARTNER},
		{Player.PARTNER, Player.LEFT},
		{Player.LEFT, Player.PLAYER}
	};

	public const int CARD_WIDTH = 96;
	public const int CARD_HEIGHT = 128;
	public static Suit Trump = Suit.UNASSIGNED;
	public enum Phase
	{
		loading,
		drafting,
		playing
	}

	public static Suit[] GetAllSuits()
	{
		Suit[] arr = new Suit[]{
			Suit.SPADES,
			Suit.CLUBS,
			Suit.DIAMONDS,
			Suit.HEARTS
		};
		return arr;
	}

	public static Rank[] GetAllRanks()
	{
		Rank[] arr = new Rank[]{
			Rank.ace,
			Rank.two,
			Rank.three,
			Rank.four,
			Rank.five,
			Rank.six,
			Rank.seven,
			Rank.eight,
			Rank.nine,
			Rank.ten,
			Rank.jack,
			Rank.queen,
			Rank.king
		};
		return arr;
	}

	// Game state Variables
	public static Phase GlobalGamePhase;
	public static List<string> CurrentHand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
