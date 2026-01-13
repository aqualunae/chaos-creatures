using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    [SerializeField]
    private int partySize;

    [SerializeField]
    private CreatureEgg[] eggs;

    private Dictionary<int, ChaosCreature> creatures;
    private Dictionary<string, CreatureEgg> labelledEggs;

    private void Awake()
    {
        creatures = new Dictionary<int, ChaosCreature>();
        for (int i = 0; i < partySize; i++)
        {
            creatures.Add(i, null);
        }

        labelledEggs = new Dictionary<string, CreatureEgg>();
        for (int i = 0; i < eggs.Length; i++)
        {
            string eggSpecies = eggs[i].GetSpecies();
            if (!labelledEggs.ContainsKey(eggSpecies))
            {
                labelledEggs.Add(eggSpecies, eggs[i]);
            }
        }
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
            if (creatures[firstCreatureIndex].GetSpecies().Equals(creatures[secondCreatureIndex].GetSpecies()))
            {
                CreatureEgg egg = labelledEggs[creatures[firstCreatureIndex].GetSpecies()];
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
