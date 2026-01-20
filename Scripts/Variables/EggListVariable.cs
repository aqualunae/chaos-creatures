using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Source of truth for the list of possible species. Only create one.
/// </summary>
[CreateAssetMenu(fileName = "Species List", menuName = "Variables/Egg List")]
public class EggListVariable : Variable<CreatureEgg[]>
{
    public Dictionary<string, CreatureEgg> GetEggDictionary()
    {
        Dictionary<string, CreatureEgg> eggDictionary = new Dictionary<string, CreatureEgg>();
        for (int i = 0; i < Value.Length; i++)
        {
            eggDictionary.Add(Value[i].Species, Value[i]);
        }
        return eggDictionary;
    }

    public Dictionary<string, ChaosCreature> GetCreatureDictionary()
    {
        Dictionary<string, ChaosCreature> creatureDictionary = new Dictionary<string, ChaosCreature>();
        for (int i = 0; i < Value.Length; i++)
        {
            creatureDictionary.Add(Value[i].Species, Value[i].Creature);
        }
        return creatureDictionary;
    }

    public CreatureEgg GetEgg(string species)
    {
        return Value.First(egg => egg.Species.Equals(species));
    }

    public ChaosCreature GetCreature(string species)
    {
        return Value.First(egg => egg.Species.Equals(species)).Creature;
    }
}
