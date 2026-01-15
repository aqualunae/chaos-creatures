# Chaos Creatures Game

## Summary

Chaos Creatures is a pet collection game about using the power of randomness and friendship to save the world! Each creature has hundreds of possible variations: discover them through exploration or grow your team by pairing creatures and hatching their eggs. Lovingly inspired by DeviantArt original species from the early 2000s.

## Scope

I would like to make Chaos Creatures into a fully playable game. It will not be possible to give it the degree of polish it needs within five months, so I am viewing the Ubisoft mentorship program as phase one of three.

1. Develop the core functionality and features of the game.
2. Create unique visual assets.
3. Expand horizontally.

### Phase One - Core Development

- Creature mechanics: encounters, battling, capture, leveling, storage, and pairing.
- Progression mechanics: saving, quests, dialogue, tutorials, cutscenes.
- Other mechanics: inventory, menus, traveling between maps, crafting.

Within each category, the mechanics are listed in order of priority.

In order to focus on the development of core systems, I will be re-using assets from my other project, Hallowed Crossing (HC). These assets include limited character animations, map tiles, and some audio. I will have unique battle sprites for the Creatures themselves. I may edit HC assets, but creating entirely new sprites is out of scope for this phase of the project.

The battle system will be relatively simple, with a limited number of skills available. I will expand on skills during a later phase.

Phase one should end with a playable demo containing two maps, 4-7 creatures, and a teaser of the plot.

### Phase Two - Assets

After the Ubisoft mentorship term has ended, I will focus on unique assets for the game.

- Animations for the Creatures (battle sprites)
- World sprites for the Creatures
- Map tiles and objects
- Inventory items
- Character sprites (battle and world)
- UI Elements

This will also be the time to work on core features that I didn't get to during the mentorship.

Phase two should end with a more polished and visually distinct version of the demo from phase one, as well as a library of assets to be used in phase three.

### Phase Three - Horizontal Expansion

With all core features in place, I'll be able to add more creatures, more battle skills, more items, more explorable maps, etc. Most of the plot progression and character development will be added during this phase.

Phases two and three may happen in parallel.

Phase three should end with a full-featured game that is ready for alpha testing.

## Code

### Entities

#### Save Data
- Creature
- Player Character

#### Static Data
- Species
- Non-Player Character
- Map
- Cutscene
- Item
- Skill
- Quest
- Shop
- Recipe
- Map

### Actions

#### Encounters
- Random Encounter
- Character Encounter
- Battle
- Use Skill
- Use Combat Item
- Use Befriending Item
- Attempt Escape
- Victory
- Defeat

#### Creature Management
- Access Creature Storage
- Store Creature
- Withdraw Creature
- Release Creature
- Edit Creature Active Skills
- View Creature Stats
- Edit Creature Equipment

#### Inventory
- Use Item Outside of Combat
- Move Item
- Discard Item
- Equip Item to Creature
- Craft

#### Pairing
- Assign Parents
- Obtain Egg
- Hatch Egg

#### Pauze Menu
- Party List
- Inventory
- Return to Main Menu
- Save
- Settings
- Quests
- Map

#### Quests
- Accept Quest
- Achieve Quest Goal
- View Quest Details
- Complete Quest
- Abandon Quest

#### Characters
- Give Quest
- Combat Encounter
- Greeting
- Rematch Challenge

#### Maps
- Static Interactable
- Ground Item
- Random Encounter
- Exit

#### Main Menu
- New Game
- Load Game
- View Saves
- Delete Save
- Settings

#### Shopkeepers
- Greeting
- Open Shop
- Buy Item
- Sell Item
- Exit Shop