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
	[Test]
	public async Task TestHandEval1()
	{
		List<string> hand = new List<string> {
			"clubs_a", "clubs_3", "clubs_4", "clubs_5"
		};
		LoadPlayedCards(hand);
		Trump = Suit.SPADES;
		_playing.ActivePlayer = Player.PLAYER;
		await Task.Delay(100);
		Player winner = _playing.HandEval();
		winner.ShouldBe(Player.PLAYER);

		// hand = new List<string> {
		// 	"clubs_a", "clubs_3", "clubs_4", "clubs_a"
		// };
		// LoadPlayedCards(hand);
		// Trump = Suit.CLUBS;
		// await Task.Delay(100);
		// winner = _playing.HandEval();
		// winner.ShouldBe(Player.LEFT);
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