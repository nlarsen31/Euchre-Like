namespace Eurchelike;

using System.Threading.Tasks;
using Godot;
using Chickensoft.GoDotTest;
using GodotTestDriver;
// using GodotTestDriver.Drivers;
using Shouldly;

using static GlobalProperties;
using static GlobalMethods;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class PlayingTests : TestClass
{
	private Playing _playing = default!;
	private Fixture _fixture = default!;

	public PlayingTests(Node testScene) : base(testScene) { }

	[SetupAll]
	public async Task SetupAll()
	{
		List<string> deck = new List<string>();
		foreach (Rank rank in GetAllRanks())
		{
			foreach (Suit suit in GetAllSuits())
			{
				deck.Add(SuitToString[(int)suit] + "_" + RankToString[(int)rank]);
			}
		}
		RandomizeList<string>(deck);
		CurrentHand = new List<string>();
		for (int i = 0; i < 13; i++)
		{
			CurrentHand.Add(deck[i]);
		}
		GD.Print("test");
		_fixture = new Fixture(TestScene.GetTree());
		_playing = await _fixture.LoadAndAddScene<Playing>();
		// _log.Print("Setup everything");
	}

	// Tests the following:
	// 1. Getters and setters of the cardContainer class
	// 2. Test the IsRight and isLeft properties.
	[Test]
	public async Task TestLeftAndRight()
	{
		CardContainer cardContainer = new CardContainer();
		cardContainer.Suit = Suit.SPADES;
		cardContainer.Rank = Rank.jack;
		Trump = Suit.SPADES;
		await Task.Delay(100);
		cardContainer.IsRight.ShouldBe(true);
		cardContainer.IsLeft.ShouldBe(false);
		Trump = Suit.CLUBS;
		cardContainer.IsLeft.ShouldBe(true);
		cardContainer.IsRight.ShouldBe(false);

		cardContainer.Suit = Suit.DIAMONDS;
		Trump = Suit.DIAMONDS;
		cardContainer.IsRight.ShouldBe(true);
		cardContainer.IsLeft.ShouldBe(false);
		Trump = Suit.HEARTS;
		cardContainer.IsLeft.ShouldBe(true);
		cardContainer.IsRight.ShouldBe(false);
	}


	// Test the operators of <
	// they consider what suit is but not what was lead.
	[Test]
	public async Task CardContainerOpps()
	{
		// result stores card1 < card2, card1 > card2, card2 < card1, card2 > card1 values
		void test(Rank rank1, Suit suit1, Rank rank2, Suit suit2, Suit trump, bool[] result)
		{
			Trump = trump;
			CardContainer card1 = new CardContainer
			{
				Rank = rank1,
				Suit = suit1
			};
			CardContainer card2 = new CardContainer
			{
				Rank = rank2,
				Suit = suit2
			};

			bool LessThan = card1 < card2;
			bool GreaterThan = card1 > card2;
			LessThan.ShouldBe(result[0]);
			GreaterThan.ShouldBe(result[1]);
			LessThan = card2 < card1;
			GreaterThan = card2 > card1;
			LessThan.ShouldBe(result[2]);
			GreaterThan.ShouldBe(result[3]);
		}

		bool LessThan, GreaterThan;
		await Task.Delay(100);
		CardContainer card1 = new CardContainer();
		CardContainer card2 = new CardContainer();

		// Left vs right of spades
		// Trump = Suit.SPADES;
		// card1.Rank = Rank.jack;
		// card1.Suit = Suit.SPADES;
		// card2.Rank = Rank.jack;
		// card2.Suit = Suit.CLUBS;
		// LessThan = card1 < card2;
		// LessThan.ShouldBe(false);
		// GreaterThan = card1 > card2;
		// GreaterThan.ShouldBe(true);
		// LessThan = card2 < card1;
		// LessThan.ShouldBe(true);
		// GreaterThan = card2 > card1;
		// GreaterThan.ShouldBe(false);

		// Left vs right of spades:
		bool[] results = { false, true, true, false };
		test(Rank.jack, Suit.SPADES, Rank.jack, Suit.CLUBS, Trump, results);

		// Left vs right of HEARTS
		Trump = Suit.HEARTS;
		card1.Rank = Rank.jack;
		card1.Suit = Suit.HEARTS;
		card2.Rank = Rank.jack;
		card2.Suit = Suit.DIAMONDS;
		LessThan = card1 < card2;
		LessThan.ShouldBe(false);
		GreaterThan = card1 > card2;
		GreaterThan.ShouldBe(true);
		LessThan = card2 < card1;
		LessThan.ShouldBe(true);
		GreaterThan = card2 > card1;
		GreaterThan.ShouldBe(false);

		// ACE vs Random trump
		Trump = Suit.SPADES;
		card1.Rank = Rank.ace;
		card1.Suit = Suit.HEARTS;
		card2.Rank = Rank.two;
		card2.Suit = Suit.SPADES;
		LessThan = card1 < card2;
		LessThan.ShouldBe(true);
		GreaterThan = card1 > card2;
		GreaterThan.ShouldBe(false);
	}

	[CleanupAll]
	public void Cleanup() => _fixture.Cleanup();

	// [Failure]
	// public void Failure() =>
	// 	GD.Print("fail");
	// //   _log.Print("Runs whenever any of the tests in this suite fail.");


	// Useful helpers to make tests
	// Takes a List<string> Load the first 4 elements into the playing.PlayedCards
	public void LoadPlayedCards(List<string> hand)
	{
		foreach (CardContainer card in _playing.PlayedCards)
		{
			if (card != null)
			{
				card.Dispose();
			}
		}

		Player player = Player.PLAYER;
		foreach (string cardStr in hand)
		{
			Tuple<Rank, Suit> tup = GetSuitRankFromString(cardStr);
			CardContainer cardContainer = new CardContainer();
			cardContainer.Rank = tup.Item1;
			cardContainer.Suit = tup.Item2;
			_playing.PlayedCards[(int)player] = cardContainer;
			player = NextPlayer(player);
		}
	}
}