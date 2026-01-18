using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    [SerializeField]
    private int partySize;

    [SerializeField]
    private CreatureEgg[] eggs;

    [SerializeField]
    private CreaturePreset[] presetParty;

    private Dictionary<int, ChaosCreature> creatures;
    private Dictionary<string, CreatureEgg> labelledEggs;

    private void Awake()
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
}
