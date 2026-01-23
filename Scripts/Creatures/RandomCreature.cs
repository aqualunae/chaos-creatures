using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "Random Creature ", menuName = "Creatures/Random")]
public class RandomCreature : ScriptableObject
{
    [SerializeField, Tooltip("Species of creature")]
    private CreatureSpecies species;

    [SerializeField, Tooltip("Probability adjustments for appearance")]
    private GeneEffect[] geneEffects;

    [SerializeField, Tooltip("Range for the level of the creature. Min 1, Max 50")]
    private Vector2Int levelRange;

    /// <summary>
    /// Using the parameters of this object, get a random creature.
    /// </summary>
    public SaveableCreature GetRandomCreature()
    {
        GeneticOdds odds = species.GetGeneticOdds(geneEffects);
        CreatureDetails details = CreatureUtility.GetDetails(odds);
        string speciesName = species.Species;
        int level = UnityEngine.Random.Range(levelRange.x, levelRange.y);
        Stats stats = species.GetBaseStats();
        for (int i = 0; i < level; i++)
        {
            stats = species.IncrementStats(stats);
        }

        return new SaveableCreature()
        {
            species = speciesName,
            creatureName = speciesName,
            level = level,
            stats = stats,
            details = details
        };
    }
}
