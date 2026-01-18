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
    public void TestLeftRight_CompareCardContainers()
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

        CardContainer leftWild = new CardContainer();
        leftWild.Suit = Suit.TRUMP;
        leftWild.Rank = Rank.left;

        CardContainer rightWild = new CardContainer();
        rightWild.Suit = Suit.TRUMP;
        rightWild.Rank = Rank.right;

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

        Assert.IsTrue(leftWild.IsLeft,
            "CardContainer should be recognized as left when rank is left.");
        Assert.IsFalse(leftWild.IsRight,
            "CardContainer should not be recognized as right when rank is left.");
        Assert.IsTrue(rightWild.IsRight,
            "CardContainer should be recognized as right when rank is right.");
        Assert.IsFalse(rightWild.IsLeft,
            "CardContainer should not be recognized as left when rank is right.");
        Assert.IsTrue(leftWild < rightWild,
            "Left wild should be less than right wild.");

        Assert.IsTrue(trumpAce < leftWild, "Left wild should be greater than ace of trump");
        Assert.IsFalse(trumpAce > leftWild, "Left wild should not be greater than ace of trump");
        Assert.IsTrue(trumpAce < rightWild, "Right wild should be greater than ace of trump");
        Assert.IsFalse(trumpAce > rightWild, "Right wild should not be greater than ace of trump");


    }

    [Test]
    public void TestCompareCardContainers_WithSameSuit()
    {
        CurrentTrump = Suit.SPADES;
        // Create a set of CardContainers with the same suit
        CardContainer aceHearts = new CardContainer();
        aceHearts.Suit = Suit.HEARTS;
        aceHearts.Rank = Rank.ace;
        CardContainer twoHearts = new CardContainer();
        twoHearts.Suit = Suit.HEARTS;
        twoHearts.Rank = Rank.two;
        CardContainer threeHearts = new CardContainer();
        threeHearts.Suit = Suit.HEARTS;
        threeHearts.Rank = Rank.three;
        CardContainer fourHearts = new CardContainer();
        fourHearts.Suit = Suit.HEARTS;
        fourHearts.Rank = Rank.four;
        CardContainer fiveHearts = new CardContainer();
        fiveHearts.Suit = Suit.HEARTS;
        fiveHearts.Rank = Rank.five;
        CardContainer sixHearts = new CardContainer();
        sixHearts.Suit = Suit.HEARTS;
        sixHearts.Rank = Rank.six;
        CardContainer sevenHearts = new CardContainer();
        sevenHearts.Suit = Suit.HEARTS;
        sevenHearts.Rank = Rank.seven;
        CardContainer eightHearts = new CardContainer();
        eightHearts.Suit = Suit.HEARTS;
        eightHearts.Rank = Rank.eight;
        CardContainer nineHearts = new CardContainer();
        nineHearts.Suit = Suit.HEARTS;
        nineHearts.Rank = Rank.nine;
        CardContainer tenHearts = new CardContainer();
        tenHearts.Suit = Suit.HEARTS;
        tenHearts.Rank = Rank.ten;
        CardContainer jackHearts = new CardContainer();
        jackHearts.Suit = Suit.HEARTS;
        jackHearts.Rank = Rank.jack;
        CardContainer queenHearts = new CardContainer();
        queenHearts.Suit = Suit.HEARTS;
        queenHearts.Rank = Rank.queen;
        CardContainer kingHearts = new CardContainer();
        kingHearts.Suit = Suit.HEARTS;
        kingHearts.Rank = Rank.king;

        Assert.IsTrue(aceHearts > twoHearts, "Ace should be greatert than Two in Hearts");
        Assert.IsTrue(threeHearts > twoHearts, "Three should be greater than Two in Hearts");
        Assert.IsTrue(fourHearts > threeHearts, "Four should be greater than Three in Hearts");
        Assert.IsTrue(fiveHearts > fourHearts, "Five should be greater than Four in Hearts");
        Assert.IsTrue(sixHearts > fiveHearts, "Six should be greater than Five in Hearts");
        Assert.IsTrue(sevenHearts > sixHearts, "Seven should be greater than Six in Hearts");
        Assert.IsTrue(eightHearts > sevenHearts, "Eight should be greater than Seven in Hearts");
        Assert.IsTrue(nineHearts > eightHearts, "Nine should be greater than Eight in Hearts");
        Assert.IsTrue(tenHearts > nineHearts, "Ten should be greater than Nine in Hearts");
        Assert.IsTrue(jackHearts > tenHearts, "Jack should be greater than Ten in Hearts");
        Assert.IsTrue(queenHearts > jackHearts, "Queen should be greater than Jack in Hearts");
        Assert.IsTrue(kingHearts > queenHearts, "King should be greater than Queen in Hearts");
        Assert.IsTrue(aceHearts > kingHearts, "Ace should be greater than King in Hearts");

        Assert.IsTrue(twoHearts < threeHearts, "Two should be less than Three in Hearts");
        Assert.IsTrue(threeHearts < fourHearts, "Three should be less than Four in Hearts");
        Assert.IsTrue(fourHearts < fiveHearts, "Four should be less than Five in Hearts");
        Assert.IsTrue(fiveHearts < sixHearts, "Five should be less than Six in Hearts");
        Assert.IsTrue(sixHearts < sevenHearts, "Six should be less than Seven in Hearts");
        Assert.IsTrue(sevenHearts < eightHearts, "Seven should be less than Eight in Hearts");
        Assert.IsTrue(eightHearts < nineHearts, "Eight should be less than Nine in Hearts");
        Assert.IsTrue(nineHearts < tenHearts, "Nine should be less than Ten in Hearts");
        Assert.IsTrue(tenHearts < jackHearts, "Ten should be less than Jack in Hearts");
        Assert.IsTrue(jackHearts < queenHearts, "Jack should be less than Queen in Hearts");
        Assert.IsTrue(queenHearts < kingHearts, "Queen should be less than King in Hearts");
        Assert.IsTrue(kingHearts < aceHearts, "King should be less than Ace in Hearts");
    }
    [Test]
    public void TestCompareCardContainers_WithSameSuitTrump()
    {
        CurrentTrump = Suit.HEARTS;
        // Create a set of CardContainers with the same suit
        CardContainer aceHearts = new CardContainer();
        aceHearts.Suit = Suit.HEARTS;
        aceHearts.Rank = Rank.ace;
        CardContainer twoHearts = new CardContainer();
        twoHearts.Suit = Suit.HEARTS;
        twoHearts.Rank = Rank.two;
        CardContainer threeHearts = new CardContainer();
        threeHearts.Suit = Suit.HEARTS;
        threeHearts.Rank = Rank.three;
        CardContainer fourHearts = new CardContainer();
        fourHearts.Suit = Suit.HEARTS;
        fourHearts.Rank = Rank.four;
        CardContainer fiveHearts = new CardContainer();
        fiveHearts.Suit = Suit.HEARTS;
        fiveHearts.Rank = Rank.five;
        CardContainer sixHearts = new CardContainer();
        sixHearts.Suit = Suit.HEARTS;
        sixHearts.Rank = Rank.six;
        CardContainer sevenHearts = new CardContainer();
        sevenHearts.Suit = Suit.HEARTS;
        sevenHearts.Rank = Rank.seven;
        CardContainer eightHearts = new CardContainer();
        eightHearts.Suit = Suit.HEARTS;
        eightHearts.Rank = Rank.eight;
        CardContainer nineHearts = new CardContainer();
        nineHearts.Suit = Suit.HEARTS;
        nineHearts.Rank = Rank.nine;
        CardContainer tenHearts = new CardContainer();
        tenHearts.Suit = Suit.HEARTS;
        tenHearts.Rank = Rank.ten;
        CardContainer jackHearts = new CardContainer();
        jackHearts.Suit = Suit.HEARTS;
        jackHearts.Rank = Rank.jack;
        CardContainer queenHearts = new CardContainer();
        queenHearts.Suit = Suit.HEARTS;
        queenHearts.Rank = Rank.queen;
        CardContainer kingHearts = new CardContainer();
        kingHearts.Suit = Suit.HEARTS;
        kingHearts.Rank = Rank.king;
        CardContainer jackDiamonds = new CardContainer();
        jackDiamonds.Suit = Suit.DIAMONDS;
        jackDiamonds.Rank = Rank.jack;

        Assert.IsTrue(aceHearts > twoHearts, "Ace should be greatert than Two in Hearts");
        Assert.IsTrue(threeHearts > twoHearts, "Three should be greater than Two in Hearts");
        Assert.IsTrue(fourHearts > threeHearts, "Four should be greater than Three in Hearts");
        Assert.IsTrue(fiveHearts > fourHearts, "Five should be greater than Four in Hearts");
        Assert.IsTrue(sixHearts > fiveHearts, "Six should be greater than Five in Hearts");
        Assert.IsTrue(sevenHearts > sixHearts, "Seven should be greater than Six in Hearts");
        Assert.IsTrue(eightHearts > sevenHearts, "Eight should be greater than Seven in Hearts");
        Assert.IsTrue(nineHearts > eightHearts, "Nine should be greater than Eight in Hearts");
        Assert.IsTrue(tenHearts > nineHearts, "Ten should be greater than Nine in Hearts");
        Assert.IsTrue(queenHearts > tenHearts, "Queen should be greater than Jack in Hearts");
        Assert.IsTrue(kingHearts > queenHearts, "King should be greater than Queen in Hearts");
        Assert.IsTrue(aceHearts > kingHearts, "Ace should be greater than King in Hearts");
        Assert.IsTrue(jackDiamonds > aceHearts, "Jack of Diamonds should be greater than Ace of Hearts");
        Assert.IsTrue(jackHearts > jackDiamonds, "Jack of Hearts should be greater than Jack of Diamonds");

        Assert.IsTrue(twoHearts < threeHearts, "Two should be less than Three in Hearts");
        Assert.IsTrue(threeHearts < fourHearts, "Three should be less than Four in Hearts");
        Assert.IsTrue(fourHearts < fiveHearts, "Four should be less than Five in Hearts");
        Assert.IsTrue(fiveHearts < sixHearts, "Five should be less than Six in Hearts");
        Assert.IsTrue(sixHearts < sevenHearts, "Six should be less than Seven in Hearts");
        Assert.IsTrue(sevenHearts < eightHearts, "Seven should be less than Eight in Hearts");
        Assert.IsTrue(eightHearts < nineHearts, "Eight should be less than Nine in Hearts");
        Assert.IsTrue(nineHearts < tenHearts, "Nine should be less than Ten in Hearts");
        Assert.IsTrue(tenHearts < queenHearts, "Jack should be less than Queen in Hearts");
        Assert.IsTrue(queenHearts < kingHearts, "Queen should be less than King in Hearts");
        Assert.IsTrue(kingHearts < aceHearts, "King should be less than Ace in Hearts");
        Assert.IsTrue(aceHearts < jackDiamonds, "Ace of Hearts should be less than Jack of Diamonds");
        Assert.IsTrue(jackDiamonds < jackHearts, "Jack of Diamonds should be less than Jack of Hearts");
    }

    [Test]
    public void TestUpgrades()
    {
        CardContainer card = new CardContainer();
        card.Rank = Rank.ace;
        card.Suit = Suit.HEARTS;

        card.ApplyUpgrade(UpgradeType.Strength);
        Assert.IsTrue(card.Rank == Rank.two, "Ace should upgrade to Two with Strength upgrade");
        card.ApplyUpgrade(UpgradeType.Strength);
        Assert.IsTrue(card.Rank == Rank.three, "Two should upgrade to Three with Strength upgrade");
        Assert.IsTrue(card.Suit == Suit.HEARTS, "Suit should remain the same after Strength upgrade");

        CurrentTrump = Suit.SPADES;
        card.ApplyUpgrade(UpgradeType.ChangeClubs);
        Assert.IsTrue(card.Suit == Suit.CLUBS, "Suit should change to Clubs after ChangeClubs upgrade");
        card.ApplyUpgrade(UpgradeType.ChangeHearts);
        Assert.IsTrue(card.Suit == Suit.HEARTS, "Suit should change to Hearts after ChangeHearts upgrade");
        card.ApplyUpgrade(UpgradeType.ChangeDiamonds);
        Assert.IsTrue(card.Suit == Suit.DIAMONDS, "Suit should change to Diamonds after ChangeDiamonds upgrade");
        card.ApplyUpgrade(UpgradeType.ChangeSpades);
        Assert.IsTrue(card.Suit == Suit.SPADES, "Suit should change to Spades after ChangeSpades upgrade");
    }
}
