using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Collection of creatures associated with a character. Saveable.
/// </summary>
public class Party : SaveableBehaviour
{
    [SerializeField, Tooltip("Maximum number of creatures")]
    private int partySize;

    [SerializeField, Tooltip("Pre-established creatures in the party.")]
    private CreatureInstance[] presetParty;

    [SerializeField]
    private GameObjectVariable storageRef;

    [SerializeField]
    private SpeciesListVariable speciesList;

    [SerializeField, Tooltip("Event that is fired when the player moves.")]
    private Vector3Event movementEvent;

    [SerializeField, Tooltip("Event that is fired when the player makes progress in the game.")]
    private StringEvent progressionTrigger;

    [SerializeField]
    private GameObjectVariable eventWindowRef;

    // Dictionary specifically so that there can be empty slots, for example if a specific creature is removed.
    private Dictionary<int, SaveableCreature> creatures;

    public Dictionary<int, SaveableCreature> Creatures
    {
        get => creatures;
    }

    public int CreatureCount
    {
        get => creatures.Count(slot => slot.Value != null);
    }

    /// <summary>
    /// Get a party member by index, for example in combat.
    /// </summary>
    public SaveableCreature GetByIndex(int index)
    {
        if (index >= partySize)
        {
            Debug.Log("Party index out of bounds.");
            return null;
        }
        return creatures[index];
    }

    /// <summary>
    /// Add a creature instance to party.
    /// </summary>
    /// <returns>True if successful.</returns>
    public int AddToParty(SaveableCreature creature)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                creatures[i] = creature;
                saveEvent.Invoke(SaveState.Save);
                return i;
            }
        }
        return -1;
    }

    public int AddToStorage(SaveableCreature creature)
    {
        return storageRef.Value.GetComponent<Party>().AddToParty(creature);
    }

    /// <summary>
    /// Replaces a party member with data of a creature of the same species. Useful for leveling up.
    /// </summary>
    /// <param name="index">Index of the party member.</param>
    /// <param name="creature">Updated creature.</param>
    /// <returns>True if successful.</returns>
    public bool UpdatePartyMember(int index, SaveableCreature creature)
    {
        if (creatures[index] == null)
        {
            Debug.Log("No creature to update.");
            return false;
        }

        if (creatures[index].species != creature.species)
        {
            Debug.Log("Not the same creature.");
            return false;
        }

        creatures[index] = creature;
        saveEvent.Invoke(SaveState.Save);
        return true;
    }

    /// <summary>
    /// Remove a creature from party by index.
    /// </summary>
    /// <returns>True if successful.</returns>
    public bool RemoveFromParty(int index)
    {
        if (creatures[index] != null)
        {
            creatures[index] = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Restore all party creatures to max health.
    /// </summary>
    public void HealAll()
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if (creatures[i] != null)
            {
                creatures[i].stats.currentHP = (int)creatures[i].stats.hp;
            }
        }
    }

    private int selectedIndex = -1;

    /// <summary>
    /// Select a creature to prepare for swapping slots.
    /// </summary>
    /// <param name="index">Selected creature slot index</param>
    public void Select(int index)
    {
        if (selectedIndex == -1)
        {
            selectedIndex = index;
        }
        else
        {
            Swap(selectedIndex, index);
            selectedIndex = -1;
        }
    }

    /// <summary>
    /// Swap the slot positions of two creatures in the same party.
    /// </summary>
    /// <param name="first">First slot index</param>
    /// <param name="second">Second slot index</param>
    private void Swap(int first, int second)
    {
        SaveableCreature firstCreature = creatures[first];
        SaveableCreature secondCreature = creatures[second];
        creatures[second] = firstCreature;
        creatures[first] = secondCreature;
    }

    public SaveableCreature Pair(int index)
    {
        if (selectedIndex == -1)
        {
            selectedIndex = index;
            return creatures[index];
        }
        else
        {
            SaveableCreature child = PairPartyCreatures(selectedIndex, index);
            selectedIndex = -1;
            return child;
        }
    }

    /// <summary>
    /// Generate an offspring based on two parent creatures.
    /// </summary>
    private SaveableCreature PairPartyCreatures(int firstCreatureIndex, int secondCreatureIndex)
    {
        if (
            firstCreatureIndex != secondCreatureIndex && 
            creatures[firstCreatureIndex] != null && creatures[secondCreatureIndex] != null &&
            creatures[firstCreatureIndex].level > 0 && creatures[secondCreatureIndex].level > 0
        )
        {
            string speciesName = creatures[firstCreatureIndex].species;
            if (speciesName.Equals(creatures[secondCreatureIndex].species))
            {
                CreatureSpecies species = speciesList.GetSpecies(speciesName);
                GeneticOdds childOdds = species.GetGeneticOdds(creatures[firstCreatureIndex], creatures[secondCreatureIndex]);
                CreatureDetails childDetails = CreatureUtility.GetDetails(childOdds, true);
                Stats stats = species.GetBaseStats();
                Equipment equipment = new Equipment()
                {
                    braceletStyle = creatures[firstCreatureIndex].equipment.braceletStyle,
                    baseColorTitle = creatures[firstCreatureIndex].equipment.baseColorTitle,
                    accentColorTitle = creatures[secondCreatureIndex].equipment.accentColorTitle,
                    charms = new string[3]
                };
                SaveableCreature child = new SaveableCreature()
                {
                    species = speciesName,
                    creatureName = $"{speciesName} Egg",
                    level = 0,
                    stats = stats,
                    details = childDetails,
                    equipment = equipment
                };
                EventWindow eventWindow = eventWindowRef.Value.GetComponent<EventWindow>();
                eventWindow.Pair(creatures[firstCreatureIndex], creatures[secondCreatureIndex]);

                Debug.Log(child.level);
                return child;
            }
        }
        return null;
    }

    private void Start()
    {
        if (movementEvent != null)
        {
            movementEvent.AddListener(OnMove);
        }
    }

    /// <summary>
    /// Reduce egg step count for all party eggs.
    /// Notify that one has hatched, if applicable.
    /// </summary>
    private void OnMove(Vector3 position)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] != null && creatures[i].details.eggSteps > 0)
            {
                creatures[i].details.eggSteps--;
                if (creatures[i].details.eggSteps == 0)
                {
                    creatures[i].level = 1;
                    progressionTrigger.Invoke($"Hatch { creatures[i].species }");
                    EventWindow eventWindow = eventWindowRef.Value.GetComponent<EventWindow>();
                    eventWindow.Hatch(this, i);
                }
            }
        }
    }

    /// <summary>
    /// Changes the name of a creature.
    /// </summary>
    /// <param name="index">Index of the creature</param>
    /// <param name="input">String to be its new name</param>
    /// <returns>True if successful</returns>
    public bool ChangeName(int index, string input)
    {
        if (creatures[index] != null && CreatureUtility.IsStringSafe(input))
        {
            creatures[index].creatureName = input;
            return true;
        }
        return false;
    }

    #region Saving

    // CreatureInstances don't save correctly, so they need to be converted to SaveableCreatures.
    // JsonUtility prefers Lists over other enumerables.
    public class PartySaveData
    {
        public List<SaveableCreature> creatures;
    }

    /// <summary>
    /// Called by Save System when it doesn't find existing save data.
    /// </summary>
    public override void OnNewGame()
    {
        // create the party using the presets, if any
        creatures = new Dictionary<int, SaveableCreature>();
        for (int i = 0; i < partySize; i++)
        {
            creatures.Add(i, null);
        }

        // if there are preset party creatures, add them to the party
        if (presetParty.Length > 0)
        {
            for (int i = 0; i < presetParty.Length; i++)
            {
                // create the creature
                SaveableCreature creature = new SaveableCreature()
                {
                    species = presetParty[i].Species.Species,
                    creatureName = presetParty[i].CreatureName,
                    level = presetParty[i].Level,
                    stats = presetParty[i].Stats,
                    details = presetParty[i].Details,
                    equipment = null
                };

                // increase its stats to be level appropriate
                for (int level = 0; level < creature.level; level++)
                {
                    creature.stats = presetParty[i].Species.IncrementStats(creature.stats);
                }

                // add it to the party
                creatures[i] = creature;
            }
        }
    }

    /// <summary>
    /// Called by Save System to write data to file.
    /// </summary>
    /// <returns>Data to write.</returns>
    public override Saveable OnSave()
    {
        List<SaveableCreature> savedCreatures = new List<SaveableCreature>();
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] != null)
            {
                savedCreatures.Add(creatures[i]);
            }
        }

        PartySaveData partySave = new PartySaveData
        {
            creatures = savedCreatures
        };
        
        string data = JsonUtility.ToJson(partySave);
        
        // id is a guid generated by SaveableBehaviour
        // adding the type for ease of searching by menus, etc
        string identifier = $"{typeof(Party)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    /// <summary>
    /// Called by Save System on Start when data is found. Converts creatures from SaveableCreatures (generic class) to CreatureInstance (scriptable object).
    /// </summary>
    /// <param name="saveable">Data to load.</param>
    public override void OnLoad(Saveable saveable)
    {
        creatures = new Dictionary<int, SaveableCreature>();
        PartySaveData saveData = JsonUtility.FromJson<PartySaveData>(saveable.data);
        for (int i = 0; i < partySize; i++)
        {
            if (saveData.creatures.Count > i)
            {
                creatures.Add(i, saveData.creatures[i]);
            }
            else
            {
                creatures.Add(i, null);
            }
        }
    }

    #endregion
}
