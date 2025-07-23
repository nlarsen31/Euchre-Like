using System;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;

[Test]
public partial class CardContainerTests : Node
{

    [Test]
    public void TestCardContainerInitialization()
    {
        CardContainer cardContainer = new CardContainer();
        Assert.IsNotNull(cardContainer, "CardContainer should be initialized.");

        // Check properties
        Assert.IsFalse(cardContainer.IsLeft, "IsLeft should be false by default.");
        Assert.IsFalse(cardContainer.IsRight, "IsRight should be false by default.");
    }

    // Right Left Tests
    [Test]
    public void TestCardSuitAndTrump()
    {
        // Set the current trump
        CurrentTrump = Suit.DIAMONDS;
        CardContainer right = new CardContainer();
        right.Suit = Suit.DIAMONDS;
        right.Rank = Rank.jack;
        CardContainer offSuitJack = new CardContainer();
        offSuitJack.Suit = Suit.CLUBS;
        offSuitJack.Rank = Rank.jack;
        CardContainer left = new CardContainer();
        left.Suit = Suit.HEARTS;
        left.Rank = Rank.jack;
        CardContainer trumpAce = new CardContainer();
        trumpAce.Suit = Suit.DIAMONDS;
        trumpAce.Rank = Rank.ace;

        Assert.IsTrue(right.IsRight,
            "CardContainer should be recognized as right when suit is trump and rank is jack.");
        Assert.IsFalse(right.IsLeft,
            "CardContainer should not be recognized as left when suit is trump and rank is jack.");

        Assert.IsFalse(offSuitJack.IsRight,
            "CardContainer should not be right when suit is not trump.");

        Assert.IsTrue(left.IsLeft,
            "CardContainer should be recognized as left when suit is trump and rank is jack and off color suit.");
        Assert.IsFalse(left.IsRight,
            "CardContainer should not be recognized as right when suit is trump and rank is jack and off color suit.");
        Assert.AreEqual(left.Suit, Suit.DIAMONDS,
            "When card is left, suit should match on color suit");

        // Compare ace and left, and right
        Assert.IsTrue(trumpAce < left, "Left of trump should be greater than ace");
        Assert.IsFalse(trumpAce > left, "Left of trump should not be greater than ace");
        Assert.IsTrue(trumpAce < right, "Right of trump should be greater than ace");
        Assert.IsFalse(trumpAce > right, "Right of trump should not be greater than ace");
        Assert.IsTrue(left < right, "Left should be less than right");
        Assert.IsFalse(left > right, "Left should not be greater than right");
    }
}
