using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Lootable
{
    public Item item;
    public int amount;

    [Tooltip("Higher numbers mean the item is more common."), Range(1, 25)]
    public int weight;
}

[CreateAssetMenu(fileName = "Loot Table ", menuName = "Items/Loot Table")]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private Lootable[] lootItems;

    /// <summary>
    /// Get a random item from the loot table
    /// </summary>
    /// <returns></returns>
    public Lootable GetLootable()
    {
        // sort the items so that highest weights are first
        Array.Sort(lootItems, (a, b) => a.weight - b.weight);

        // get the total of the weight
        int totalWeight = lootItems.Sum(loot => loot.weight);

        // pick a random number
        int rand = UnityEngine.Random.Range(0, totalWeight);

        // count up by the weight
        int counter = 0;
        for (int i = 0; i < lootItems.Length; i++)
        {
            counter += lootItems[i].weight;

            // when the counter exceeds the random number, return the corresponding lootable
            if (counter > rand)
            {
                return lootItems[i];
            }
        }

        // failing that, return the most common item
        return lootItems[0];
    }
}
