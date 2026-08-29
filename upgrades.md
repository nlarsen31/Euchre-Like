# Upgrades and Consumables Plan

## Consumables System

Consumables are temporary per-round effects the player can activate from 3 slots. They are purchased between rounds using gold earned from leftover tricks.

### Pricing

- **Tier 1**: 10 gold
- **Tier 2**: 20 gold
- **Tier 3**: 40 gold

### Consumables by Tier

#### Tier 1 (10 gold)
- **SwapLeft** — swap a card with the left player's hand
- **SwapRight** — swap a card with the right player's hand
- **SwapPartner** — swap a card with your partner's hand

#### Tier 2 (20 gold)
- **ToTrump** — convert a card to the current trump suit
- **RankBoost** — raise a card's rank by 1 (5→6, Q→K, etc.)
- **SuitSwap** — change a card's suit to a specific suit
- **BowerPromote** — turn a card into a bower (becomes a top-tier trump card)

#### Tier 3 (40 gold)
- **BlockTrump** — opponent cannot play trump for one trick
- **AllOrNothing** — double gold earned from next trick, but lose double if you lose it
- **Gamble** — randomly boost or reduce a card's value
- **GuaranteedWin** — automatically win the next trick

### Implementation Status

- [x] Data model (enum, lookup dictionaries, GlobalProperties)
- [x] AppliedConsumable class created
- [x] Scene wiring (Playing._Ready loads consumables)
- [x] RevertAppliedConsumables() structure in place
- [ ] Individual consumable effect implementations
- [ ] Consumable shop/purchasing UI
- [ ] Activation UI (player selects which consumable to use)
