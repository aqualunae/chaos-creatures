using System.Collections.Generic;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using Unity.VisualScripting;
using UnityEngine;

public class Party : SaveableBehaviour
{
    [SerializeField]
    private int partySize;

    [SerializeField]
    private CreatureInstance[] presetParty;

    private Dictionary<int, CreatureInstance> creatures;
    private bool initialized = false;

    private void Awake()
    {
        instances.Add(this);
    }

    public override void OnNewGame()
    {
        Debug.Log("new file");
        creatures = new Dictionary<int, CreatureInstance>();
        for (int i = 0; i < partySize; i++)
        {
            if (presetParty.Length > i)
            {
                creatures.Add(i, presetParty[i]);
            }
            else
            {
                creatures.Add(i, null);
            }
        }
        initialized = true;
    }

    public CreatureInstance GetIndex(int index)
    {
        if (index >= partySize)
        {
            Debug.Log("Party index out of bounds.");
            return null;
        }
        return creatures[index];
    }

    public bool AddToParty(ChaosCreature creature)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                CreatureInstance instance = new CreatureInstance(creature.Species, creature.Name, creature.Level, creature.Stats, creature.Details);
                creatures[i] = instance;
                return true;
            }
        }
        return false;
    }

    public bool AddToParty(CreatureInstance instance)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                creatures[i] = instance;
                return true;
            }
        }
        return false;
    }

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
    /// Not used; where is the egg going?
    /// </summary>
    public bool PairPartyCreatures(int firstCreatureIndex, int secondCreatureIndex)
    {
        if (creatures[firstCreatureIndex] != null && creatures[secondCreatureIndex] != null)
        {
            if (creatures[firstCreatureIndex].Species.Equals(creatures[secondCreatureIndex].Species))
            {
                // CreatureEgg egg = labelledEggs[creatures[firstCreatureIndex].Species];
                // CreatureEgg instantiatedEgg = Instantiate(egg);
                // instantiatedEgg.InitializePair(creatures[firstCreatureIndex], creatures[secondCreatureIndex]);
                // instantiatedEgg.transform.localScale = Vector2.one;
                // instantiatedEgg.transform.position = new Vector2(0.48f, -0.32f);
                // return true;
            }
        }
        return false;
    }

    #region Saving

    public class PartySaveData
    {
        public List<SaveableCreature> creatures;
    }

    public override Saveable OnSave()
    {
        List<SaveableCreature> savedCreatures = new List<SaveableCreature>();
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] != null)
            {
                savedCreatures.Add(new SaveableCreature()
                {
                    species = creatures[i].Species,
                    creatureName = creatures[i].CreatureName,
                    level = creatures[i].Level,
                    stats = creatures[i].Stats,
                    details = creatures[i].Details
                });
            }
        }

        PartySaveData partySave = new PartySaveData
        {
            creatures = savedCreatures
        };
        
        string data = JsonUtility.ToJson(partySave);
        Debug.Log(data);
        string identifier = $"{typeof(Party)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        Debug.Log("loading");
        creatures = new Dictionary<int, CreatureInstance>();
        PartySaveData saveData = JsonUtility.FromJson<PartySaveData>(saveable.data);
        for (int i = 0; i < partySize; i++)
        {
            if (saveData.creatures.Count > i)
            {
                creatures.Add(i, new CreatureInstance(saveData.creatures[i].species, saveData.creatures[i].creatureName, saveData.creatures[i].level, saveData.creatures[i].stats, saveData.creatures[i].details));
            }
            else
            {
                creatures.Add(i, null);
            }
        }
        initialized = true;
    }

    #endregion
}
