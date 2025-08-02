using System;
using System.Collections.Generic;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;


[Test]
public partial class CardSelectionTests : Node
{
    private CardSelection _cardSelection;

    [BeforeAll]
    public void BeforeAll()
    {
        GD.Print("Setting up CardSelectionTests...");
        try
        {
            // Initialize any required resources or state here
            var packedScene = ResourceLoader.Load<PackedScene>("res://2DraftScene/CardSelection.tscn");
            _cardSelection = (CardSelection)packedScene.Instantiate();
            if (_cardSelection == null)
            {
                GD.PrintErr("Failed to instantiate CardSelection from scene.");
                throw new Exception("CardSelection instantiation failed.");
            }
            AddChild(_cardSelection); // Add to scene tree so Godot initializes it
            GD.Print("CardSelectionTests setup complete.");
        }
        catch (Exception e)
        {
            GD.PrintErr("Error during CardSelectionTests setup: ", e);
            throw;
        }
    }

    [Test]
    public void CardSelectionPassingTest()
    {
        Assert.IsTrue(true, "This is a passing test for card selection.");
    }

    [Test]
    public void CardSelectionTestSetup()
    {
        _cardSelection.clearCards();
        Assert.IsNotNull(_cardSelection, "CardSelection should not be null after setup.");

        CardContainer leftleft = new CardContainer();
        leftleft.Suit = Suit.HEARTS;
        leftleft.Rank = Rank.ace;
        _cardSelection.AddCard(leftleft);

        CardContainer leftRight = new CardContainer();
        leftRight.Suit = Suit.HEARTS;
        leftRight.Rank = Rank.two;
        _cardSelection.AddCard(leftRight);

        CardContainer rightleft = new CardContainer();
        rightleft.Suit = Suit.HEARTS;
        rightleft.Rank = Rank.three;
        _cardSelection.AddCard(rightleft);

        CardContainer rightright = new CardContainer();
        rightright.Suit = Suit.HEARTS;
        rightright.Rank = Rank.four;
        _cardSelection.AddCard(rightright);

        List<string> cards = _cardSelection.GetCardSelectionString();
        Assert.AreEqual("hearts_a", cards[0], "First card should be hearts ace.");
        Assert.AreEqual("hearts_2", cards[1], "Second card should be hearts two.");
        Assert.AreEqual("hearts_3", cards[2], "Third card should be hearts three.");
        Assert.AreEqual("hearts_4", cards[3], "Fourth card should be hearts four.");
    }

    [AfterAll]
    public void AfterAll()
    {
        GD.Print("Cleaning up CardSelectionTests...");
        if (_cardSelection != null)
        {
            RemoveChild(_cardSelection);
        }
    }
}