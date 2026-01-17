# Euchre-Like

Euchre Rogue LIke made in Godot

## Getting started

- Run ./gd-local-setup.sh - this will install godot to the repo folder
- Download and install .NET with the current version of GoDot <https://dotnet.microsoft.com/en-us/download> 8.0 is recommended.

## Changes Starting on July 7 to be included in 1.0.2

### Long term updates

- How to play screen

### Known bugs

- Sometimes when the Upgrade Scene loads one of the options doesn't get replaced and shows "option <n>" Most recently this happened after winning 5 tricks.

### Add Modifiers

- Prerequisite changes
  - After first hand, we need to keep track of the "other cards" so we deal the same cards every time
  - <del>Add a "modifier screen" that displays options, for starters this can be just a UI on top of the game</del>

- Purposed Modifiers
  - Increase the rank of 1 card in your hand (common)
  - Change the suit of one card in your hand (common)
  - Make one card a jack in your hand (uncommon)
  - Make one card always Trump (uncommon)
  - Make one card always left (rare)
  - Make one card always right (rare)

## Work log

### Jan 17 2026

- Fix readme issues, change to windows launch file.
- Try to run 4.4
- Add traking of what cards are non-players for randomly setting up Playing scene.

#### Upgrades not working

- I have added the tracking of what goes into the oppo's hands, but for some reason it is not rendering after the first round.
- I just ran through and "change to spades" worked.
- Fixed the issue, Needed to add set up of players hand on second load of playing.

### August 3

- Add unit tests for Draft Scene

### August 2

- Add tests for CardSelection

### July 28

- Add HandOfCards tests
- Added question, now sure how we want to handle "packedScenes"

### July 12

- Make Suit rotate in a cycle
- Display next suit on Upgrade screen.

### July 11

- Add Transition from Playing to Upgrade
- Add a Label for each hand

### July 6 v1.0.1

Current State:

- Upgraded to 4.4 took out the unit tests
- Draft 13 cards, selecting between 3 face up and one face down card
- Play one round
- Revert PC to select random card to play that is legal
- Once required tricks are won, up requirement by 2 and restart
