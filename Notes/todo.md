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

- created save system
- party became saveable behaviour

## Next Up

- scene change: start menu to campground and back
- player prefab
- player movement
- camera following player movement
- npc prefab

- connect opponent to combat screen

- create scriptable object for specific creature, with current hp, level, name, etc
    - instantiate preset programmatically or create a separate class?
- connect party to that instead of the species base prefab
- renderers to connect the species prefab to the scriptable object (combat, menu, etc)
- how can menus access the save data / saveablebehaviour instances?

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