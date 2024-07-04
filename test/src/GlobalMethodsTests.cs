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

public class GlobalMethodsTest : TestClass
{
   private Playing _playing = default!;
   private Fixture _fixture = default!;

   public GlobalMethodsTest(Node testScene) : base(testScene) { }


   [SetupAll]
   public async Task SetupAll()
   {
      _fixture = new Fixture(TestScene.GetTree());
   }

   [Test]
   public async Task NextPlayerTest()
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
}