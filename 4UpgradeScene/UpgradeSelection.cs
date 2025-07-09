using Godot;
using System;
using System.Collections.Generic;
using System.Collections;

public partial class UpgradeSelection : Node2D
{

    // Member declarations
    private int _upgradeIndex;
    private List<Button> _UpgradeButtons;

    public override void _Ready()
    {
        Button button1 = GetNode<Button>("UpgradeButton1");
        Button button2 = GetNode<Button>("UpgradeButton2");
        Button button3 = GetNode<Button>("UpgradeButton3");
        _UpgradeButtons = new List<Button> { button1, button2, button3 };
        _upgradeIndex = 0;
    }

    public void SetUnusedUpgrade(string upgrade)
    {
        if (_upgradeIndex < _UpgradeButtons.Count)
        {
            _UpgradeButtons[_upgradeIndex].Text = upgrade;
            _UpgradeButtons[_upgradeIndex].Visible = true;
            _upgradeIndex++;
        }
        else
        {
            GD.Print("No more upgrade buttons available.");
        }
    }

    
    public void OnUpgradeSelected(int bind)
    {
        GD.Print("Upgrade selected: " + bind);
    }

}
