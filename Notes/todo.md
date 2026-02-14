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

- allow saveables to trigger saves on state change
- method for saveables to remove themselves from save system
- created view party menu
- ability to switch between main pauze menu and party menu
- health and experience sliders slide gradually
- when combat ends, the player clicks end to close window
- (as opposed to it closing automatically after 2 seconds)
- moved "experience to next level" calculation to CreatureUtility

- fixed: creature renderer sort order for softshells not working as intended
- implemented warping
- refactored code so that player can be instantiated anywhere and maintain existing stats
- entrance point variable can be set to convey player location when switching scenes
- exit point outside of main collider to ensure player doesn't ricochet
- fixed: game sometimes crashes on load of movers

### Week of February 4

- player snapped to grid
- player returned to snapped position on collision
- saveables check if they are already present

- created display keybind component
- updated start menu to allow new game or load game
- updated transition from start menu to loaded game to have more accurate positioning

- improved keyboard navigation
    - set Enter and Space to do the same thing both on UI and Player (Overworld) maps
    - added scripting to select appropriate buttons
    - removed unnecessary references to event system
    - made sure that the control scheme switches from player to ui and back appropriately
    - added pauze to the ui control scheme to allow keyboard un-pauzing
- ensured that player skills become unavailable on defeat
- upgraded pauze event from bool to enum
- created interaction indicator
- added inventory
- lootable items on the ground can be added to the inventory
- inventory saves and loads correctly

- randomized loot tables
- created more items
- improved inventory checking on pickup
- inventory window and slot
- selected item panel on inventory window

- items menu in the combat screen
- tab switcher and enable listener components
- improved combat keyboard navigation
    - particularly when switching tabs
    - fixed an issue with attempting to select inactive inventory slots
- fixed: dialogue skipping lines
- some combat items can be used
- created UseItemResult class

- bracelet logic

- switching creatures party positions
- fixed: creature details don't display after second combat instance
    - related to using a new method to switch tabs
- fixed: dialogue cannot be progressed after exiting combat
- fixed: clicks not registering on start menu after quitting from the pauze menu
    - related to having the ui action map incorrectly disabled while on the overworld

### Week of February 11

- moved combat item use logic up the chain to Combat Window and Combat Inventory Window, instead of having it in Inventory Slot
- created Item Listener component, which accepts an index from Inventory Slot and passes it to Inventory Window or Combat Inventory Window
- on capturing creature, select the end button
- display an appropriate message when inventory is empty
- pauze menu always opens to the highest level menu
- interactions fields display action to take
- move interact to space
- use only one action map at a time
- pauze on dialogue

- switch creatures during combat
- prevent switching to a creature that has 0 HP
- prevent sending out a creature with 0 HP
- refactor creature slot to be less entangled with its parent
- rename item listener to selection listener
- automatically open creature switching window on defeat
- do not open creature switching window if whole party has 0 hp
- switching creatures during combat ends player's turn

## Next Up

### Issues

- loading in the hyper arcade and then moving to campgrounds causes a crash

### Features

- handle friendship when party is full
- creature storage

- move items between slots in the same inventory
- move items to storage
- discard items
- item use logic
- lootable items on timer; saveable delta time
- code commenting in items folder
- creature equipment

- responsive ui scaling

- add audio effects: https://www.bfxr.net/

- menus access the save master as needed

- calculate and display creature rarity

- logic for handling combat
    - aspect influence
    - skills that influence stats
    - make sure stat changes are temporary
    - first skill use to be determined by speed

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
- key cap by Arthur Shlain from <a href="https://thenounproject.com/browse/icons/term/key-cap/" target="_blank" title="key cap Icons">Noun Project</a> (CC BY 3.0)

## Pauzing

- when the game starts, it is not pauzed

- pauzing:
    - opens the pauze menu
    - disables all movers

## Use Item Flow

- Inventory Window has a Selection Listener component
- Inventory Window draws Inventory Slots
- when an Inventory Slot is selected or activated, it tells the Selection Listener its index
- Selection Listener relays the index back to the Inventory Window
- Inventory Window executes item display or use logic