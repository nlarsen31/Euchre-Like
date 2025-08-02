using System;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;

[Test]
public partial class DraftSceneTesets : Node
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

    [AfterAll]
    public void AfterAll()
    {
        RemoveChild(_draftScene);
    }
}