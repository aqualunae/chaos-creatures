using System.Collections.Generic;
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

    // Dictionary specifically so that there can be empty slots, for example if a specific creature is removed.
    private Dictionary<int, SaveableCreature> creatures;

    public Dictionary<int, SaveableCreature> Creatures
    {
        get => creatures;
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
    public bool AddToParty(SaveableCreature creature)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                creatures[i] = creature;
                saveEvent.Invoke(SaveState.Save);
                return true;
            }
        }
        return false;
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

    /// <summary>
    /// Not used; where is the egg going?
    /// </summary>
    public bool PairPartyCreatures(int firstCreatureIndex, int secondCreatureIndex)
    {
        // if (creatures[firstCreatureIndex] != null && creatures[secondCreatureIndex] != null)
        // {
        //     if (creatures[firstCreatureIndex].Species.Equals(creatures[secondCreatureIndex].Species))
        //     {
        //         CreatureEgg egg = labelledEggs[creatures[firstCreatureIndex].Species];
        //         CreatureEgg instantiatedEgg = Instantiate(egg);
        //         instantiatedEgg.InitializePair(creatures[firstCreatureIndex], creatures[secondCreatureIndex]);
        //         instantiatedEgg.transform.localScale = Vector2.one;
        //         instantiatedEgg.transform.position = new Vector2(0.48f, -0.32f);
        //         return true;
        //     }
        // }
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
