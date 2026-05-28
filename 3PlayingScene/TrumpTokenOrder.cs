using System;
using System.Xml.Serialization;
using Godot;
using static GlobalProperties;

public partial class TrumpTokenOrder : Node2D
{
    private Chip chip0;
    private Chip chip1;
    private Chip chip2;
    private Chip chip3;

    public override void _Ready()
    {
        GD.Print("TrumpTokenOrder _Ready");
        chip0 = GetNode<Chip>("%Chip0");
        chip1 = GetNode<Chip>("%Chip1");
        chip2 = GetNode<Chip>("%Chip2");
        chip3 = GetNode<Chip>("%Chip3");
    }

    public void SetTrumpOrder(GlobalProperties.Suit[] trumpOrder)
    {
        if (trumpOrder.Length != 4)
        {
            GD.PrintErr("Trump order must have exactly 4 suits.");
            return;
        }

        GD.Print("Setting trump order");
        string chip0Str = SuitToString[(int)(Suit)trumpOrder[0]];
        string chip1Str = SuitToString[(int)(Suit)trumpOrder[1]];
        string chip2Str = SuitToString[(int)(Suit)trumpOrder[2]];
        string chip3Str = SuitToString[(int)(Suit)trumpOrder[3]];

    }
}
