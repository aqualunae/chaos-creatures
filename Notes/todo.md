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

## Next Up

- scene change: start menu to campground and back
- player prefab
- player movement
- camera following player movement
- npc prefab

- connect opponent

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
