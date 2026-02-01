# Tasks

## What I've Done

### Week of January 14

- outlined MVP
- filled out Agreement
- imported and set up tilemaps
- created Campground scene
- started wrangling pixel perfect camera
- created start menu scene

- set up start menu buttons
- set up combat screen layout
- created creature presets
- created initial skills

- ability to add creature presets to party
- combat window, combat stats, skill button scripts
- combat window can access party creatures
- creature renderer for ui
- added default ui event system
- stats converted from int to float to facilitate gradual increase

- created player and guide prefabs
- created save system
- party became saveable behaviour
- moved species list to its own variable
- refactored creatures to separate species and rendering from instance data

- added commenting to scripts in the Creatures folder (not Combat)
- added commenting to scripts in the Combat, Characters, and Systems folders

### Week of January 21

- refactored creatures to separate species from individual from rendering
- separated creature renderer (sprites, can be used in overworld) from creature camera (creates an image for use in the ui)

- implemented randomcreature
- moved some functionality from creaturegg (legacy) to creaturespecies and creatureutility
- created scriptable events
- added random encounters on grass only
- encounter code commenting

- added basic pauze menu
- implemented pauzing
- can return to menu

- added currentHP and exp to SaveableCreature
- created initial damage and experience formulas
- string event to update combat logs
- starting to implement skills
- fixed crash on new game

- opponent skill use
- victory and defeat
- preliminary warp system
- handled self-targeting skills
- fixed experience error confusing ^ with Math.Pow()
- added panel to view creature details

- creature random adoption

### Week of January 28

- added interaction system
- adjusted input for interaction via mouse or keyboard/controller
- added dialogue window
- converted mover to a saveable behaviour
- centered camera to player
- confined camera

## Next Up

- creature renderer sort order for softshells not working as intended

- menu to view party
- menus access the save master

- allow saveables to trigger saves on state change
- method for saveables to remove themselves from save system

- continue work on warp system

- implement items
- code commenting in items folder

- logic for handling combat
    - aspect influence
    - skills that influence stats
    - make sure stat changes are temporary

## Combat Flow

- creatures you can encounter stored in scriptable objects
- array of creature encounters in the scene
- random encounter possibility -> gridTile.OnTriggerEnter -> opens combat window
- combat window
    - initialize encountered creature
    - initialize player's party creature
    - show player's available skills
    - show both creatures' health, level, and name
    - bonus: toggle details panel
- bonus: player or opponent first depending on speed stat
- skill button triggers skill
- opponent turn happens
- skills
    - log window to display skill effects
    - bonus: find pixel vfx
- flee -> close combat window
- victory and defeat pop-ups

### Math

#### Experience

- Experience total required to reach next level: (level * 5)^3 // level 1: 125, level 5: 15625
- Experience to get from one level to the next: (((level - 1) * 5)^3) - ((level * 5)^3) // level 1: 125, level 5: 7625
- Experience earned from one victory: (opponentLevel * 3)^3 // level 1: 27, level 5: 3375

#### Damage

##### Example Stats

- Paradise Wolf Dash Lvl 1: 15 power, 0 moveCrit, 4 attack, 15 hp, 2 userCrit, 4 defense
- Paradise Wolf Spark Lvl 3: 20 power, 0.1 moveCrit, 10 attack, 35 hp, 3 userCrit, 5 defense
- Paradise Wolf Flare Lvl 5: 30 power, 0 crit, 13 attack, 46 hp, 4 userCrit, 6 defense

##### Formulas

- Add aspect modifiers

- Base Damage: (movePower * 0.5) * (((attack - defense) * 0.01) + 1)
- Randomized Damage: Random.Range(baseDamage * 0.8, baseDamage * 1.2)
- Crit Chance: (userCrit * 0.03) + moveCrit
- Crit Effect: damage * ((userCrit * 0.1) + 1)

#### Friendship

- Befriending Rate:

## Save Flow

- source of truth: central save system
    - can be accessed by menus
    - can read/write to file easily
    - may duplicate information stored in various objects
- during gameplay
    - saveable monobehavior objects report their state to the save system
    - menus can draw on the save system's information
- on save
    - save system writes all the data it contains to a file
- on load
    - save system reads the file
    - saveable objects receive their state from the system

### Resources

- https://www.answermind.blog/unlock-unity-master-persistent-data-path-flawless-game-saves
- https://stackoverflow.com/a/65495834

## Pauzing

- when the game starts, it is not pauzed

- pauzing:
    - opens the pauze menu
    - disables all movers

