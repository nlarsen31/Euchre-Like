using System;
using GD_NET_ScOUT;
using Godot;
using static GlobalProperties;

[Test]
public partial class ChipTests : Node
{
    [Export]
    private PackedScene Chip;

    private Chip _chip;

    [BeforeAll]
    public void BeforeAll()
    {
        GD.Print("Setting up ChipTests");
        _chip = (Chip)Chip.Instantiate();
        GD.Print("ChipTests setup complete");
        _chip.SetAnimation(Suit.HEARTS);
        _chip.SetLeadPosition(Player.PLAYER);
    }

    [Test]
    public void TestLeadPosition()
    {
        _chip.SetLeadPosition(Player.PLAYER);
        Assert.AreEqual(_chip.Position, new Vector2(1824, 888), "Lead position for Player should be (1824, 888)");
        _chip.SetLeadPosition(Player.RIGHT);
        Assert.AreEqual(_chip.Position, new Vector2(1856, 320), "Lead position for Left should be (1856, 320)");
        _chip.SetLeadPosition(Player.PARTNER);
        Assert.AreEqual(_chip.Position, new Vector2(720, 56), "Lead position for Right should be (720, 56)");
        _chip.SetLeadPosition(Player.LEFT);
        Assert.AreEqual(_chip.Position, new Vector2(48, 760), "Lead position for Partner should be (48, 760)");

        _chip.SetLeadPosition(Player.UNDEFINED);
        Assert.AreEqual(_chip.Position, new Vector2(48, 760), "Lead position for UNDEFINED should not change from last valid position");
    }
}
