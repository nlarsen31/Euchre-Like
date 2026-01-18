# Upgrades

## Current status of all upgrades

- Strength (Works)
- ChangeHearts (Initial Development done)
- ChangeDiamonds (Initial Development done)
- ChangeClubs (Initial Development done)
- ChangeSpades (Initial Development done)
- ChangeToJack (Initial Development done)
- NonJackToTrump (Not Started)
- ChangeToAlwaysLeft (Not Started)
  - Need to add support for always left
- ChangeToAlwaysRight (Not Started)
  - Need to add support for always right

## Development notes (Notes in reverse order)

- I have noticed that sometimes the third upgrade option is just `option 3`
- (Resolved) Noticed a bug, when changing a card to clubs, and selecting that card, it never left my hand. It was the right bower
  - Seems to be that a modified card is not leaving the hand.
  - This had to do with when there was two identical cards in the hand. It would not find the second one because it would find the first non-visible card.
- (Resolved) Known issue, when you have two of the same card, they both disappear when one is played..
- Got Strength working added somethings for next upgrades
- Got the upgrade screen to go back to playing with keeping track of the cards that are not players.
