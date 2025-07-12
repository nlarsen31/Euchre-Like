using Godot;
using System;
using System.Collections.Generic;
using System.Collections;
using static GlobalProperties;

public partial class UpgradeSelection : Node2D
{

    // Member declarations
    private int _upgradeIndex;
    private List<Button> _UpgradeButtons;
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
        _UpgradeButtons = new List<Button> { button1, button2, button3 };
        _upgradeIndex = 0;
    }

    public void SetUnusedUpgrade(string upgrade, UpgradeType upgradeType)
    {
        if (_upgradeIndex < _UpgradeButtons.Count)
        {
            _UpgradeButtons[_upgradeIndex].Text = upgrade;
            _UpgradeButtons[_upgradeIndex].Visible = true;
            _RandomGeneratedUpgrades[_upgradeIndex] = upgradeType;
            _upgradeIndex++;
        }
        else
        {
            GD.Print("No more upgrade buttons available.");
        }
    }

    public void OnUpgradeSelected(int bind)
    {
        GD.Print("Upgrade pressed: " + bind);

        for (int i = 0; i < _UpgradeButtons.Count; i++)
        {
            if (i != bind)
                _UpgradeButtons[i].Visible = false;
            else
            {
                EmitSignal(SignalName.UpgradeSelected, (int)_RandomGeneratedUpgrades[i]);
            }
        }
    }

    public void DisableButtons()
    {
        foreach (Button button in _UpgradeButtons)
        {
            button.Disabled = true;
        }
    }
}
