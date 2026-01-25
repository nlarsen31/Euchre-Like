using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using static GlobalProperties;

public partial class UpgradeSelection : Node2D
{

    // Member declarations
    private int _upgradeIndex;

    private List<CardContainer> _DisplayedUpgrades = new List<CardContainer>();
    private UpgradeType[] _RandomGeneratedUpgrades = new UpgradeType[3]
    {
        UpgradeType.Strength,
        UpgradeType.ChangeHearts,
        UpgradeType.ChangeDiamonds
    };

    // Signals going to other scenes
    [Signal]
    public delegate void UpgradeSelectedEventHandler();

    public override void _Ready()
    {
        Button button1 = GetNode<Button>("UpgradeButton1");
        Button button2 = GetNode<Button>("UpgradeButton2");
        Button button3 = GetNode<Button>("UpgradeButton3");
        CardContainer upgrade1 = GetNode<CardContainer>("Upgrade1");
        CardContainer upgrade2 = GetNode<CardContainer>("Upgrade2");
        CardContainer upgrade3 = GetNode<CardContainer>("Upgrade3");
        if (button1 == null || button2 == null || button3 == null ||
            upgrade1 == null || upgrade2 == null || upgrade3 == null)
        {
            GD.PrintErr("One or more upgrade buttons or cards not found in UpgradeSelection scene.");
        }
        _DisplayedUpgrades = new List<CardContainer> { upgrade1, upgrade2, upgrade3 };
        _upgradeIndex = 0;
    }

    public void ConnectUpgradeCards(Callable callable)
    {
        for (int i = 0; i < _DisplayedUpgrades.Count; i++)
        {
            CardContainer upgradeCard = _DisplayedUpgrades[i];
            upgradeCard.Selectable = true;
            upgradeCard.Connect("CardSelected", callable);
        }
    }
    public void DisconnectUpgradeCards(Callable callable)
    {
        for (int i = 0; i < _DisplayedUpgrades.Count; i++)
        {
            CardContainer upgradeCard = _DisplayedUpgrades[i];
            upgradeCard.Selectable = false;
            upgradeCard.Disconnect("CardSelected", callable);
        }
    }

    public void SetUnusedUpgrade(string upgrade, UpgradeType upgradeType)
    {
        GD.Print("Setting upgrade: " + upgrade + " of type " + upgradeType.ToString() + " at index " + _upgradeIndex);
        if (_upgradeIndex < _DisplayedUpgrades.Count)
        {
            CardContainer upgradeCard = _DisplayedUpgrades[_upgradeIndex];
            upgradeCard.SetUpgradeAnimation(upgradeType);
            _upgradeIndex++;
        }
        else
        {
            GD.Print("No more upgrade cards available.");
        }
    }

    public void OnUpgradeSelected(int bind)
    {
        GD.Print("Upgrade pressed: " + bind);
    }
}
