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
    string chip0Str = "";
    string chip1Str = "";
    string chip2Str = "";
    string chip3Str = "";

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
        chip0Str = SuitToString[(int)(Suit)trumpOrder[0]];
        chip1Str = SuitToString[(int)(Suit)trumpOrder[1]];
        chip2Str = SuitToString[(int)(Suit)trumpOrder[2]];
        chip3Str = SuitToString[(int)(Suit)trumpOrder[3]];

        chip0.SetAnimation(chip0Str);
        chip1.SetAnimation(chip1Str);
        chip2.SetAnimation(chip2Str);
        chip3.SetAnimation(chip3Str);
    }

    public void SetActiveTrump(int index)
    {
        string deactivateSuffix = "_inactive";
        if (index == 0)
        {
            chip0.SetAnimation(chip0Str);
            chip1.SetAnimation(chip1Str + deactivateSuffix);
            chip2.SetAnimation(chip2Str + deactivateSuffix);
            chip3.SetAnimation(chip3Str + deactivateSuffix);
        }
        if (index == 1)
        {
            chip0.SetAnimation(chip0Str + deactivateSuffix);
            chip1.SetAnimation(chip1Str);
            chip2.SetAnimation(chip2Str + deactivateSuffix);
            chip3.SetAnimation(chip3Str + deactivateSuffix);
        }
        if (index == 2)
        {
            chip0.SetAnimation(chip0Str + deactivateSuffix);
            chip1.SetAnimation(chip1Str + deactivateSuffix);
            chip2.SetAnimation(chip2Str);
            chip3.SetAnimation(chip3Str + deactivateSuffix);
        }
        if (index == 3)
        {
            chip0.SetAnimation(chip0Str + deactivateSuffix);
            chip1.SetAnimation(chip1Str + deactivateSuffix);
            chip2.SetAnimation(chip2Str + deactivateSuffix);
            chip3.SetAnimation(chip3Str);
        }
    }
}