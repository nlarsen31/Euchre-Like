using System;
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
        var _packedScene = ResourceLoader.Load<PackedScene>("res://0CommonScene/HandOfCards.tscn");
        _handOfCards = (HandOfCards)_packedScene.Instantiate();
    }


    [Test]
    public void TestAddRandomHand()
    {
        try
        {
            if (_handOfCards == null)
            {
                throw new InvalidCastException("Scene instance could not be cast to HandOfCards.");
            }
            AddChild(_handOfCards); // Add to scene tree so Godot initializes it
            GD.Print("Successfully cast scene to HandOfCards");
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to cast scene to HandOfCards: ", e);
            throw;
        }
        GD.Print(_handOfCards.NumberOfCardsLeftToPlay);
        // _handOfCards.addRandomHand();
        // Assert.IsTrue(_handOfCards.NumberOfCardsLeftToPlay > 0,
        //     "There should be cards in hand after adding a random hand");
    }

    // TODO Add more tests

    [AfterAll]
    public void AfterAll()
    {
        RemoveChild(_handOfCards);
    }



}
