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
	public static void LeftRightTests()
	{
		CardContainer cardContainer = new CardContainer();
		cardContainer.Suit = Suit.SPADES;
		cardContainer.Rank = Rank.jack;
		Trump = Suit.SPADES;
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
	public static void CardContainerOperatorTests()
	{
		// await Task.Delay(100);
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

			card1.Dispose();
			card2.Dispose();
		}

		// Left vs right of spades:
		test(Rank.jack, Suit.SPADES, Rank.jack, Suit.CLUBS, Suit.SPADES,
			new bool[] { false, true, true, false });

		test(Rank.jack, Suit.HEARTS, Rank.jack, Suit.DIAMONDS, Suit.HEARTS,
			new bool[] { false, true, true, false });

		test(Rank.ace, Suit.HEARTS, Rank.two, Suit.SPADES, Suit.SPADES,
			new bool[] { true, false, false, true });
	}

	[Test]
	public async Task HandEvalTests()
	{
		void test(Rank[] ranks, Suit[] suits, Player winner, Player leadPlayer, Suit trump)
		{
			Trump = trump;
			CardContainer card1 = new CardContainer
			{
				Rank = ranks[0],
				Suit = suits[0]
			};
			CardContainer card2 = new CardContainer
			{
				Rank = ranks[1],
				Suit = suits[1]
			};
			CardContainer card3 = new CardContainer
			{
				Rank = ranks[2],
				Suit = suits[2]
			};
			CardContainer card4 = new CardContainer
			{
				Rank = ranks[3],
				Suit = suits[3]
			};
			// _playing.PlayedCardsArr[0] = card1;
			_playing.PlayCard(Player.PLAYER, card1.ToString());
			// _playing.PlayedCardsArr[1] = card2;
			_playing.PlayCard(Player.RIGHT, card2.ToString());
			// _playing.PlayedCardsArr[2] = card3;
			_playing.PlayCard(Player.PARTNER, card3.ToString());
			// _playing.PlayedCardsArr[3] = card4;
			_playing.PlayCard(Player.LEFT, card4.ToString());

			Player activePlayer = leadPlayer;
			activePlayer = NextPlayer(activePlayer);
			activePlayer = NextPlayer(activePlayer);
			activePlayer = NextPlayer(activePlayer);
			_playing.ActivePlayer = activePlayer;

			Player winnerPred = _playing.HandEval();
			winnerPred.ShouldBe(winner);
		}
		await Task.Delay(100);

		// test top 4 trump
		test(new Rank[] { Rank.king, Rank.jack, Rank.jack, Rank.ace },
			  new Suit[] { Suit.SPADES, Suit.SPADES, Suit.CLUBS, Suit.SPADES },
			  Player.RIGHT,   // winner
			  Player.RIGHT,   // lead
			  Suit.SPADES);

		// test trump against an ace
		test(new Rank[] { Rank.ace, Rank.king, Rank.two, Rank.ace },
			  new Suit[] { Suit.SPADES, Suit.SPADES, Suit.CLUBS, Suit.SPADES },
			  Player.PARTNER, // winner
			  Player.PLAYER,  // lead
			  Suit.CLUBS);

		// test left against an non trump
		test(new Rank[] { Rank.ace, Rank.jack, Rank.jack, Rank.ace },
			  new Suit[] { Suit.SPADES, Suit.SPADES, Suit.DIAMONDS, Suit.SPADES },
			  Player.PARTNER, // winner
			  Player.RIGHT,   // lead
			  Suit.HEARTS);

		// test left against an non trump
		test(new Rank[] { Rank.ace, Rank.jack, Rank.jack, Rank.jack },
			  new Suit[] { Suit.SPADES, Suit.SPADES, Suit.DIAMONDS, Suit.HEARTS },
			  Player.LEFT, // winner
			  Player.PLAYER,   // lead
			  Suit.HEARTS);

		// Test from playing game manually
		test(new Rank[] { Rank.ace, Rank.ten, Rank.king, Rank.seven },
				new Suit[] { Suit.DIAMONDS, Suit.HEARTS, Suit.HEARTS, Suit.DIAMONDS },
				Player.PLAYER,
				Player.PLAYER,
				Suit.SPADES);

		test(new Rank[] { Rank.ace, Rank.ten, Rank.queen, Rank.eight },
				new Suit[] { Suit.SPADES, Suit.SPADES, Suit.SPADES, Suit.SPADES },
				Player.PLAYER,
				Player.PLAYER,
				Suit.DIAMONDS);
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
		foreach (CardContainer card in _playing.PlayedCardsArr)
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
			_playing.PlayedCardsArr[(int)player] = cardContainer;
			player = NextPlayer(player);
		}
	}
}