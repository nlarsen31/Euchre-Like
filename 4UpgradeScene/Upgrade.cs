using Godot;
using System;
using System.Collections.Generic;
using System.Collections;
using static GlobalProperties;
using static GlobalMethods;

public partial class Upgrade : Node2D
{

	[Export]
	private PackedScene CardContainer;

    // Member declarations
    private HandOfCards _HandOfCards;
    private UpgradeSelection _UpgradeSelection;
    private UpgradeType[] _upgrades = new UpgradeType[3];
    private Chip _TrumpChip;

    // Objects in Scene
    private Timer _CardSelectedTimer;

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

    public override void _Ready()
    {
        // Member variables
		_HandOfCards = GetNode<HandOfCards>("HandOfCards");
        _CardSelectedTimer = GetNode<Timer>("CardSelectedTimer");
        _TrumpChip = GetNode<Chip>("NextTrumpChip");
        if (CurrentTrump == Suit.UNASSIGNED)
        {
            CurrentTrump = Suit.HEARTS; // Default to Hearts if no trump is set
        }
        _TrumpChip.SetAnimation(NextTrump(CurrentTrump));
        if (CurrentHand == null)
        {
            _HandOfCards.addRandomHand();
        }
        else
        {
            foreach (string s in CurrentHand)
            {
                Tuple<Rank, Suit> tup = GetSuitRankFromString(s);
                _HandOfCards.addCard(tup.Item1, tup.Item2);
            }
        }
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
            string upgradeString = UpgradeToString[upgrade];
            _UpgradeSelection.SetUnusedUpgrade(upgradeString, upgrade);

            _upgradeChangeMap[rarity].RemoveAt(upgradeIdx);
            _upgrades[i] = upgrade;
        }

        _UpgradeSelection.Connect("UpgradeSelected", new Callable(this, "UpgradeSelectedCallback"));
    }

    public void UpgradeSelectedCallback(int UpgradeType)
    {
        // Handle the upgrade selection
        // TODO: Store the selected upgrade and prompt user to pick a card to upgrade
        UpgradeType upgrade = (UpgradeType)UpgradeType;
        Callable callable = new Callable(this, "CardSelectedCallback");
        _HandOfCards.ConnectVisibleCards(callable, upgrade);
        _UpgradeSelection.DisableButtons();
    }

    public void CardSelectedCallback(string cardName)
    {
        _CardSelectedTimer.Start();
    }
    public void CardSelectedTimerCallback()
    {
        CurrentHand = _HandOfCards.ExportHand();
        RequiredTricks = RequiredTricks + 2;
        CurrentTrump = NextTrump(CurrentTrump);
        GetTree().ChangeSceneToFile("res://3PlayingScene/Playing.tscn");
    }
}
