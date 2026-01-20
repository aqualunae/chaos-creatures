using System.Collections.Generic;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEngine;

public class Party : SaveableBehaviour
{
    [SerializeField]
    private int partySize;

    [SerializeField]
    private CreatureEgg[] eggs;

    [SerializeField]
    private CreaturePreset[] presetParty;

    private Dictionary<int, ChaosCreature> creatures;
    private Dictionary<string, CreatureEgg> labelledEggs;
    private bool initialized = false;

    private void Awake()
    {
        instances.Add(this);
        labelledEggs = new Dictionary<string, CreatureEgg>();
        for (int i = 0; i < eggs.Length; i++)
        {
            string eggSpecies = eggs[i].Species;
            if (!labelledEggs.ContainsKey(eggSpecies))
            {
                labelledEggs.Add(eggSpecies, eggs[i]);
            }
        }
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        creatures = new Dictionary<int, ChaosCreature>();
        for (int i = 0; i < partySize; i++)
        {
            if (presetParty.Length > i)
            {
                ChaosCreature presetCreature = presetParty[i].Creature;
                presetCreature.Name = presetParty[i].CreatureName;
                presetCreature.SetStats(presetParty[i].Stats, presetParty[i].Level);
                presetCreature.Details = presetParty[i].Details;
                creatures.Add(i, presetCreature);
            }
            else
            {
                creatures.Add(i, null);
            }
        }
    }

    public ChaosCreature GetFirst()
    {
        return creatures[0];
    }

    public bool AddToParty(ChaosCreature creature)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                creatures[i] = creature;
                return true;
            }
        }
        return false;
    }

    public bool AddToParty(CreaturePreset preset)
    {
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] == null)
            {
                ChaosCreature presetCreature = preset.Creature;
                presetCreature.Name = preset.CreatureName;
                presetCreature.SetStats(preset.Stats, preset.Level);
                presetCreature.Details = preset.Details;
                creatures[i] = presetCreature;
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

    public bool PairPartyCreatures(int firstCreatureIndex, int secondCreatureIndex)
    {
        if (creatures[firstCreatureIndex] != null && creatures[secondCreatureIndex] != null)
        {
            if (creatures[firstCreatureIndex].Species.Equals(creatures[secondCreatureIndex].Species))
            {
                CreatureEgg egg = labelledEggs[creatures[firstCreatureIndex].Species];
                CreatureEgg instantiatedEgg = Instantiate(egg);
                instantiatedEgg.InitializePair(creatures[firstCreatureIndex], creatures[secondCreatureIndex]);
                instantiatedEgg.transform.localScale = Vector2.one;
                instantiatedEgg.transform.position = new Vector2(0.48f, -0.32f);
                return true;
            }
        }
        return false;
    }

    #region Saving

    // class-specific save data
    [System.Serializable]
    public class CreatureSaveData
    {
        public string species;
        public CreatureDetails details;
        public Stats stats;
        public string creatureName;
        public int level;
    }

    public class PartySaveData
    {
        public List<CreatureSaveData> creatures;
    }

    public override Saveable OnSave()
    {
        List<CreatureSaveData> savedCreatures = new List<CreatureSaveData>();
        for (int i = 0; i < partySize; i++)
        {
            if (creatures[i] != null)
            {
                CreatureSaveData saveData = new CreatureSaveData()
                {
                    species = creatures[i].Species,
                    details = creatures[i].Details,
                    stats = creatures[i].Stats,
                    creatureName = creatures[i].Name,
                    level = creatures[i].Level
                };
                savedCreatures.Add(saveData);
            }
        }

        PartySaveData partySave = new PartySaveData
        {
            creatures = savedCreatures
        };
        
        string data = JsonUtility.ToJson(partySave);
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
        creatures = new Dictionary<int, ChaosCreature>();
        PartySaveData saveData = JsonUtility.FromJson<PartySaveData>(saveable.data);
        for (int i = 0; i < partySize; i++)
        {
            if (saveData.creatures.Count > i)
            {
                ChaosCreature partyCreature = labelledEggs[saveData.creatures[i].species].Creature;
                partyCreature.Name = saveData.creatures[i].creatureName;
                partyCreature.SetStats(saveData.creatures[i].stats, saveData.creatures[i].level);
                partyCreature.Details = saveData.creatures[i].details;
                creatures.Add(i, partyCreature);
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
