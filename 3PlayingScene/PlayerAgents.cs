using Godot;
using System.Collections.Generic;
using static GlobalProperties;
using static GlobalMethods;


public class PlayerAgents
{
   private static CardContainer PickRandomCard(List<CardContainer> playerHand)
   {
      int randomIdx = randy.Next(0, playerHand.Count);
      return playerHand[randomIdx];
   }
   private static List<CardContainer> GetPlayableCards(List<CardContainer> iHand, Suit iLead)
   {
      List<CardContainer> playable = new List<CardContainer>();
      bool hasVoid = true;
      foreach (CardContainer card in iHand)
      {
         if (card.Suit == iLead)
         {
            hasVoid = false;
            playable.Add(card);
         }
      }
      if (hasVoid) return iHand;
      return playable;
   }

   /*
   Rules:
      1. Plays lowest trump if able to trump
      2. Plays off suit aces with the lead
      3. Always plays highest card possible
   */
   public static CardContainer RightTurn(List<CardContainer> rightHand, Suit iLead)
   {
      if (Debugging) GD.Print("[Enter] RightTurn");
      List<CardContainer> playable = GetPlayableCards(rightHand, iLead);
      if (Debugging) GD.Print("[Leave] RightTurn");
      return PickRandomCard(playable);
   }
   // Default Left will hold trump till the end
   public static CardContainer LeftTurn(List<CardContainer> leftHand, Suit iLead)
   {
      if (Debugging) GD.Print("[Enter] LeftTurn");
      List<CardContainer> playable = GetPlayableCards(leftHand, iLead);
      if (Debugging) GD.Print("[Leave] LeftTurn");
      return PickRandomCard(playable);
   }
   // Partner will play completely randomly
   public static CardContainer PartnerTurn(List<CardContainer> partnerHand, Suit iLead)
   {
      if (Debugging) GD.Print("[Enter] PartnerTurn");
      List<CardContainer> playable = GetPlayableCards(partnerHand, iLead);
      if (Debugging) GD.Print("[Leave] PartnerTurn");
      return PickRandomCard(playable);
   }
}