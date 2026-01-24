# Euchre-Like

Euchre Rogue LIke made in Godot

## Getting started

- Run ./gd-local-setup.sh - this will install godot to the repo folder
- Download and install .NET with the current version of GoDot <https://dotnet.microsoft.com/en-us/download> 8.0 is recommended.

## Long term updates

- Add speed settings on the main screen
- Add better scoreboard
- How to play screen
- Refactor playing to be clear. Each timer needs to be more clear on what it is waiting for. Document first then code
- Brain storm how to make the game longer
- How do we make the game harder to win
  - Can we make the opponents smarter?
  - How can we incorporate making changes to your partner?

## Known bugs

- [Resolved] Sometimes when the Upgrade Scene loads one of the options doesn't get replaced and shows "option <n>" Most recently this happened after winning 5 tricks.
  - Last time this happened the options were `No Jack to Trump` `Change to Jack` `option 3`

## Work log

### Jan 23

- Make the scoreboard a better looking thing.

### Jan 18

- (Planned out) Plan out the work for upgrading cards
- (Coded and tested) Try to code out Strength
- Finished upgrades and end scenes. There is a TODO: to clean up some bad code.

### Jan 17 2026

- Fix readme issues, change to windows launch file.
- Try to run 4.4
- Add tracking of what cards are non-players for randomly setting up Playing scene.

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
