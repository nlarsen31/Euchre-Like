namespace Eurchelike;

using System.Threading.Tasks;
using Godot;
using Chickensoft.GoDotTest;
using GodotTestDriver;
// using GodotTestDriver.Drivers;
using Shouldly;

using static GlobalProperties;
using static GlobalMethods;
using static PlayerAgents;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class AgentTests : TestClass
{
   private Playing _playing = default!;
   private Fixture _fixture = default!;

   public AgentTests(Node testScene) : base(testScene) { }


   [Test]
   public static void RightAgentTests()
   {
      void Test(Suit iTrump, Suit iLead, string pick, params string[] cards)
      {
         List<CardContainer> rightHand = new List<CardContainer>();
         foreach (string card in cards)
         {
            Tuple<Rank, Suit> tup = GetSuitRankFromString(card);
            CardContainer cardContainer = new CardContainer();
            cardContainer.Rank = tup.Item1;
            cardContainer.Suit = tup.Item2;
            rightHand.Add(cardContainer);
         }

         Trump = iTrump;
         CardContainer choice = RightTurn(rightHand, iLead);
         choice.ToString().ShouldBe(pick);

      }

      Test(Suit.SPADES, // Trump
            Suit.UNASSIGNED, // Lead
            "clubs_j", // pick
            "spades_9", "diamonds_8", "clubs_7", "clubs_j");
      Test(Suit.HEARTS, // Trump
            Suit.UNASSIGNED, // Lead
            "spades_a", // pick
            "spades_a", "diamonds_8", "clubs_7", "clubs_j");


      Test(Suit.HEARTS, // Trump
            Suit.SPADES, // Lead
            "spades_a", // pick
            "spades_a", "diamonds_8", "clubs_7", "spades_4", "clubs_9");
   }

   [Test]
   public static void LeftAgentTests()
   {

      void Test(Suit iTrump, Suit iLead, string pick, params string[] cards)
      {
         List<CardContainer> leftHand = new List<CardContainer>();
         foreach (string card in cards)
         {
            Tuple<Rank, Suit> tup = GetSuitRankFromString(card);
            CardContainer cardContainer = new CardContainer();
            cardContainer.Rank = tup.Item1;
            cardContainer.Suit = tup.Item2;
            leftHand.Add(cardContainer);
         }

         Trump = iTrump;
         CardContainer choice = LeftTurn(leftHand, iLead);
         choice.ToString().ShouldBe(pick);

      }
      Test(Suit.SPADES, // Trump
            Suit.UNASSIGNED, // Lead
            "clubs_7", // pick
            "spades_9", "diamonds_8", "clubs_7", "clubs_j");

      Test(Suit.HEARTS, // Trump
            Suit.UNASSIGNED, // Lead
            "clubs_7", // pick
            "spades_a", "diamonds_8", "clubs_7", "clubs_j");


      Test(Suit.HEARTS, // Trump
            Suit.SPADES, // Lead
            "spades_4", // pick
            "spades_a", "diamonds_8", "clubs_7", "spades_4", "clubs_9");
   }
}