using Godot;
using System;
using System.Collections.Generic;
using System.Collections;

public partial class Upgrade : Node2D
{

	[Export]
	private PackedScene CardContainer;

    // Member declarations
    private HandOfCards _HandOfCards;
    private UpgradeSelection _UpgradeSelection;

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare
    }
    public enum UpgradeType
    {
        Strength,
        ChangeHearts,
        ChangeDiamonds,
        ChangeClubs,
        ChangeSpades,
        ChangeToJack,
        NoJackToTrump,
        ChangeToLeftBower,
        ChangeToRightBower
    }
    private Dictionary<Rarity, List<UpgradeType>> _upgradeChangeMap = new Dictionary<Rarity, List<UpgradeType>>()
    {
        { Rarity.Common, new List<UpgradeType>
            {
                UpgradeType.Strength,
                UpgradeType.ChangeHearts,
                UpgradeType.ChangeDiamonds,
                UpgradeType.ChangeClubs,
                UpgradeType.ChangeSpades,
            }
        },
        { Rarity.Uncommon, new List<UpgradeType>
            {
                UpgradeType.ChangeToJack,
                UpgradeType.NoJackToTrump,
            }
        },
        { Rarity.Rare, new List<UpgradeType>
            {
                UpgradeType.ChangeToLeftBower,
                UpgradeType.ChangeToRightBower
            }
        }
    };
    private Dictionary<UpgradeType, string> _upgradeToString = new Dictionary<UpgradeType, string>()
    {
        { UpgradeType.Strength, "Strength" },
        { UpgradeType.ChangeHearts, "Change Hearts" },
        { UpgradeType.ChangeDiamonds, "Change Diamonds" },
        { UpgradeType.ChangeClubs, "Change Clubs" },
        { UpgradeType.ChangeSpades, "Change Spades" },
        { UpgradeType.ChangeToJack, "Change to Jack" },
        { UpgradeType.NoJackToTrump, "No Jack to Trump" },
        { UpgradeType.ChangeToLeftBower, "Change to Left Bower" },
        { UpgradeType.ChangeToRightBower, "Change to Right Bower" }
    };




    public override void _Ready()
    {
        // Member variables
		_HandOfCards = GetNode<HandOfCards>("HandOfCards");
        _HandOfCards.addRandomHand();
        _HandOfCards.DrawHand();
        _UpgradeSelection = GetNode<UpgradeSelection>("UpgradeSelection");

        for (int i = 0; i < 3; i++)
        {

            Random random = new Random();
            double randomNumber = random.NextDouble();
            Rarity rarity = Rarity.Rare;
            // 0 -0.5 is common
            if (randomNumber < 0.5)
            {
                // Common upgrade
                rarity = Rarity.Common;
            }
            // 0.5 - 0.9 is uncommon
            else if (randomNumber < 0.9)
            {
                // Uncommon upgrade
                rarity = Rarity.Uncommon;
            }
            // else we are rare

            int upgradeIdx = random.Next(0, _upgradeChangeMap[rarity].Count);
            UpgradeType upgrade = _upgradeChangeMap[rarity][upgradeIdx];
            string upgradeString = _upgradeToString[upgrade];
            _UpgradeSelection.SetUnusedUpgrade(upgradeString);

            _upgradeChangeMap[rarity].RemoveAt(upgradeIdx);
        }

    }
}
