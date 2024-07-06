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

public class GlobalMethodsTests : TestClass
{
   // private Playing _playing = default!;
   private Fixture _fixture = default!;

   public GlobalMethodsTests(Node testScene) : base(testScene) { }


   [SetupAll]
   public async Task SetupAll()
   {

      await Task.Delay(100);
      _fixture = new Fixture(TestScene.GetTree());
   }

   [Test]
   public static void NextPlayerTest()
   {
      Player next = NextPlayer(Player.LEFT);
      next.ShouldBe(Player.PLAYER);

      next = NextPlayer(Player.PLAYER);
      next.ShouldBe(Player.RIGHT);

      next = NextPlayer(Player.RIGHT);
      next.ShouldBe(Player.PARTNER);

      next = NextPlayer(Player.PARTNER);
      next.ShouldBe(Player.LEFT);
   }

   [Test]
   public static void SuitTest()
   {

      Trump = Suit.CLUBS;
      CardContainer cardContainer = new CardContainer();
      cardContainer.Rank = Rank.jack;
      cardContainer.Suit = Suit.SPADES;
      cardContainer.Suit.ShouldBe(Suit.CLUBS);

      cardContainer.Rank = Rank.ace;
      cardContainer.Suit.ShouldBe(Suit.SPADES);

      Trump = Suit.DIAMONDS;
      cardContainer.Rank = Rank.jack;
      cardContainer.Suit = Suit.HEARTS;
      cardContainer.Suit.ShouldBe(Suit.DIAMONDS);
   }
}