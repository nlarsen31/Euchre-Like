using System;
using System.Collections.Generic;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;

[Test]
public partial class DraftSceneTests : Node
{
    private DraftScene _draftScene;

    [BeforeAll]
    public void BeforeAll()
    {
        GD.Print("Setting up CardSelectionTests...");
        try
        {
            var packedScene = ResourceLoader.Load<PackedScene>("res://2DraftScene/DraftScene.tscn");
            _draftScene = (DraftScene)packedScene.Instantiate();
            if (_draftScene == null)
            {
                GD.PrintErr("Failed to instantiate CardSelection from scene.");
                throw new Exception("CardSelection instantiation failed.");
            }
            AddChild(_draftScene); // Add to scene tree so Godot initializes it
            GD.Print("CardSelectionTests setup complete.");
        }
        catch (Exception e)
        {
            GD.PrintErr("Error during CardSelectionTests setup: ", e);
            throw;
        }
    }

    [Test]
    public void DraftScenePassingTest()
    {
        Assert.IsTrue(true, "This is a passing test for card selection.");
    }

    [Test]
    public void DraftSceneSelectCardsTest()
    {
        _draftScene.DisplayNextCards(4);

        var cardSelection = _draftScene.GetNode<CardSelection>("CardSelection");
        List<string> cardSelectionCards = cardSelection.GetCardSelectionString();
        string firstCard = cardSelectionCards[0];

        // Select the first card
        _draftScene.SelectCardCallback(firstCard);

        // Check that the card has been selected
        HandOfCards hand = _draftScene.GetNode<HandOfCards>("HandOfCards");
        Assert.AreEqual(1, hand.NumberOfCardsLeftToPlay, "One card should b e added to the hand after selection.");

        // Check the value of the one card is correct
        List<string> cardsInHand = hand.ExportHand();
        Assert.IsTrue(cardsInHand.Contains(firstCard), "The selected card should be in the hand.");
    }

    [Test]
    public void DraftSceneRandomRanksTest()
    {
        _draftScene.CurrentDraftingMode = DraftScene.DraftingMode.RANK_BASED;
    }

    [AfterAll]
    public void AfterAll()
    {
        RemoveChild(_draftScene);
    }
}