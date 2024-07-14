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

   // Default Right will play every trump if it can
   public static CardContainer RightTurn(List<CardContainer> rightHand)
   {
      if (Debugging) GD.Print("[Enter] RightTurn");
      if (Debugging) GD.Print("[Leave] RightTurn");
      return PickRandomCard(rightHand);
   }
   // Default Left will hold trump till the end
   public static CardContainer LeftTurn(List<CardContainer> leftHand)
   {
      if (Debugging) GD.Print("[Enter] LeftTurn");
      if (Debugging) GD.Print("[Leave] LeftTurn");
      return PickRandomCard(leftHand);
   }
   // Partner will play completely randomly
   public static CardContainer PartnerTurn(List<CardContainer> partnerHand)
   {
      if (Debugging) GD.Print("[Enter] PartnerTurn");
      if (Debugging) GD.Print("[Leave] PartnerTurn");
      return PickRandomCard(partnerHand);
   }
}