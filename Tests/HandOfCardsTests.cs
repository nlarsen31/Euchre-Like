using System;
using System.Collections.Generic;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;

[Test]
public partial class HandOfCardsTests : Node
{
    private HandOfCards _handOfCards;
    PackedScene _packedScene;


    [BeforeAll]
    public void BeforeAll()
    {
        try
        {
            var _packedScene = ResourceLoader.Load<PackedScene>("res://0CommonScene/HandOfCards.tscn");
            _handOfCards = (HandOfCards)_packedScene.Instantiate();
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to load HandOfCards scene: ", e);
            throw;
        }
        AddChild(_handOfCards); // Add to scene tree so Godot initializes it
    }


    [Test]
    public void TestAddRandomHand()
    {
        _handOfCards.addRandomHand();
        Assert.IsTrue(_handOfCards.NumberOfCardsLeftToPlay > 0, "Random hand should have cards");
        _handOfCards.clearCards();
        Assert.IsTrue(_handOfCards.NumberOfCardsLeftToPlay == 0, "Hand should be empty after clearing");
    }

    [Test]
    public void TestAddingSpecificCards()
    {
        _handOfCards.addCard(Rank.ace, Suit.HEARTS);
        _handOfCards.addCard(Rank.two, Suit.CLUBS);
        Assert.IsTrue(_handOfCards.NumberOfCardsLeftToPlay == 2, "Hand should have two cards after adding specific cards");
        _handOfCards.addCard(Rank.three, Suit.SPADES);
        Assert.IsTrue(_handOfCards.NumberOfCardsLeftToPlay == 3, "Hand should have three cards after adding another card");
        List<string> exportedCards = _handOfCards.ExportHand();
        Assert.IsTrue(exportedCards.Count == 3, "Exported hand should contain three cards");
        Assert.IsTrue(exportedCards.Contains("hearts_a"), "Exported hand should contain 'ace of hearts'");
        Assert.IsTrue(exportedCards.Contains("clubs_2"), "Exported hand should contain 'two of clubs'");
        Assert.IsTrue(exportedCards.Contains("spades_3"), "Exported hand should contain 'three of spades'");

    }

    [AfterAll]
    public void AfterAll()
    {
        RemoveChild(_handOfCards);
    }



}
