# Tasks

## Next Up

### Issues

- storage item slot indices display incorrectly
- opening the storage window by keyboard navigation clicks the first item instead of just selecting it
    - moving initialization functions from onenable to start prevents them from running when the window is reopened, but does prevent the first item from being clicked

#### Path Feedback

"a few more little cues here and there i think would basically be enough for me. a clear outline of the, idk? hit box? of whatever i need to click on, or the label also being clickable. a "return to combat" label in the menu."

- labels should be clickable as well as what they're labelling
- add "return" button to combat party screen

### Features

- trainer battles
    - pacing of sending out the opponent's next creature is off
    - opponent quote on victory or defeat
    - do not allow fleeing from trainer battles, or heal opponent when you do

- creature pairing
    - animation when pairing creatures
    - animation when hatching creature egg

- name creatures

- creature storage
    - text displaying when storage is empty

- save to local storage - player prefs
    - https://discussions.unity.com/t/how-do-i-save-the-game-progress-in-webgl/851849/10

- creature calls on adoption screen
- maybe also on selection in party

- item use logic
    - separate healing items from combat items?
- creature equipment
    - add and remove charms
- implement item drag
- removing item from inventory:
    - delete it
    - "slot" for discarding items
    - confirmation popup
- highlight item when it is being used or moved
- items that affect local genetic probability

- responsive ui scaling
    - canvas scaler & set transforms to stretch as needed

- menus access the save master as needed?

- logic for handling combat
    - aspect influence
    - skills that influence stats
    - make sure stat changes are temporary
    - first skill use to be determined by speed

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
- changed interact to space
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

- updated commenting in combat folder
- creature camera has configurable output size
- updated game state to allow more options
    - prevents hitting pauze to leave adoption or combat early
    - allows hitting pauze to leave dialogue early
- made sure the start menu has the ui action map enabled
- fixed: opening the inventory window does not select any buttons, which breaks keyboard-only navigation
- starting to navigate inventory interactions in the pauze menu

- created move mode toggle in inventory
- ability to move items between inventory slots
- fixed: moving an item does not select it
- added item storage window
- added item storage crate
- item storage has its own game state

- addded time system
- put lootable items on timer
- made lootable items saveable
- fixed: game crashes when party with less than maximum creatures is defeated (checking stats of null)
- fixed: storage window moves the first item selected (selection from previous opening persisted)
- fixed: game crashes when attempting to pick up an item while the first inventory slot is empty (checking stack size of null)
- fixed: items not usable in combat (checking wrong index)
- when swapping two of the same item (in the same inventory or across storage), merge the stacks
- fixed: swapping an item with itself deletes the item (need to check same)

### Week of February 18

- found and added skill animations
- added sprite swapper that dynamically updates a single skill animation to any skill animation
- moved skill use logic from skill button to combat window
- added continuous movement by holding down move keys

- created sound effects with bfxr.net
- added sound effects to skills and creatures
- updated the combat window to call sounds
- added music to scenes and combat

- updated start menu buttons to look activated when selected
- separated warp point assignment from warp point activation
- updated warp point assignment to only run if the scene name isn't null or empty
- added menus to the systems prefab
- fully implemented CreatureSpecies.GetGeneticOdds(GeneEffect[] effects)
- added more commenting to the items folder
- added creature overview window, equipment panel, equipment slot, and bracelet renderer
- standardized item colors

### Week of February 19

- refactored movement to be on update
- fixed: hedgefrogs never appear in the adoption window

- added progression system and progression checker
- updated combat window to call progression triggers appropriately
- updated party to automatically level preset party creatures
- added progression system reference to scene initializer
- updated dialogue and dialogue window to have a challenge option
- implemented character encounter
- updated initialize combat window to accept an opponent party OR a random creature

- fixed: progression checker fires before progression system loads
- menu stats displays victory and loss count
- guide mentions the possibility of challenging him later
- fixed: shader for creature cameras not included in build

### Week of March 4

- fixed(?): loading in the hyper arcade and then moving to campgrounds causes a crash sometimes
    - check if the entrance point variable is being misused
    - only call warp point assignment if gameobject.activeinhierarchy
    - set warp points to dirty in the editor

- implemented skill details using the selection listener
- adjusted audio to prevent track from resetting on opening and closing menus
- added background music to the start menu
- added more randomized creatures to the spawn table
- updated the warp self component to work cross-map
- fixed keyboard navigation on stats panel of pauze menu

- calculated and implemented creature rarity score

### Week of March 11

- implemented creature storage (attached to tent)

- fixed: going to a panel other than skills and fleeing combat prevents the use of skills in subsequent combat instances, until you change scenes
- fixed: clickable area of tent is too small
- fixed: "challenge" is misspelled in the dialogue
- disable storage action buttons when switching windows
- message to player when moving last creature out of party
- moved creature storage from tent to doghouse

### Week of March 18

- fixed: scrollbars don't appear on storage (required content fitter component)
- fixed: custom rendered materials don't obey masks (added stencil to shader)
- fixed: not enough space for scrollbar on inventory storage

- updated world object artwork to allow color selection
- focusing a creature on the storage page now scrolls to that creature
- fixed: keyboard trap when viewing creature details
- creatures befriended while party is full are now sent to storage, space allowing
- improved visible logging on creature storage, including actions and slot count
- updated selection listener and creature slot interactions
- added scroll-ability to party window and combat party window

- refactor party window and combat party window to draw the slots so that party size can be changed
- refactor party window, combat party window, and creature storage window to be related and consistent
- implement creature pairing
- implement creature release

# Notes

## Credits

- music from [Tallbeard Studios](https://tallbeard.itch.io/)
- sound effects from [bfxr](https://www.bfxr.net/)
- visual effects from [BDragon1727](https://bdragon1727.itch.io/1050-rpg-effects-64x64)

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

#### Creature Rarity Score

- sample data for paradise wolf
- combinations:
    - 800 400 000
    - 2 primary
    - 2 secondary
    - 2 tertiary
    - 4 base colors x 3 usages
    - 5 accent colors x 5 usages
    - 7 body patterns
    - 6 primary patterns
    - 6 secondary patterns
    - 2 tertiary patterns
- weighted combinations
    - 1.423828125e+21 = 1423828125000000000000
    - 1/5 primary, secondary, tertiary
    - 1/45 base color x 3 usages
    - 1/50 accent color x 5 usages
    - 1/100 body, primary, secondary pattern
    - 1/10 tertiary
- weighted combinations (without reducing)
    - 729000000000000000000000000000 = 7.29e+29

- chance of getting most common creature of species:
    - multiply all fractions:
        - 1.2661379039232e-4 = 0.00012661379039232 = 1 in 7898
    - multiply all numbers on each side, then divide:
        - 38050725888000000000000000/729000000000000000000000000000 = 3.8050725888e+25/7.29e+29 = 114688/2197265625 = 5.219578311111111e-5 = 0.00005219578311111111 = 1 in 19,158.63582066127
    - add all numbers on each side, then divide:
        - 790/1470 = 0.5374149659863946 = 1 in 1.860759493670886 = score of 1
    
- chance of getting rarest creature of species:
    - multiply all fractions:
        - 1.2288e-22 = 0.00000000000000000000012288 = 1 in 8138020833333333000000
    - multiply all numbers on each side, then divide:
        - 122880000/729000000000000000000000000000 = 1 in 5932617187500000000000 = 5.9326171875e+21 = 1.2288e+8/7.29e+29
    - add all numbers on each side, then divide:
        - 137/1470 = 0.0931972789115646 = 1 in 10.72992700729927 = score of 10

- chance of getting most common creature of species:
    - 80/100 fluffy tail
    - 80/100 angel wings
    - 60/100 spiky hair
    - 60/90 body & primary black base
    - 60/90 secondary black base
    - 60/90 tertiary black base
    - 40/100 body red accent
    - 40/100 eyes red
    - 40/100 primary red accent (?)
    - 40/100 secondary red accent
    - 40/100 tertiary red accent
    - 40/100 body points
    - 40/100 primary points
    - 40/100 secondary points
    - 70/100 tertiary solid

- chance of getting rarest creature of species:
    - 20/100 claw tail
    - 20/100 demon wings
    - 40/100 curly hair
    - 2/90 body & primary pink base
    - 2/90 secondary pink base
    - 2/90 tertiary pink base
    - 2/100 body pink accent
    - 2/100 eyes pink
    - 2/100 primary pink accent (?)
    - 2/100 secondary pink accent
    - 2/100 tertiary pink accent
    - 1/100 body heartbreak
    - 1/100 primary heartbreak
    - 1/100 secondary heartbreak
    - 30/100 tertiary stripes

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
- https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui

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