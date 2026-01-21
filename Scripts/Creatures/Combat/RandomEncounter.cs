using Assets.Scripts.Creatures;
using UnityEngine;

public class RandomEncounter : ScriptableObject
{
    [SerializeField, Tooltip("Species of creature")]
    private CreatureEgg egg;

    [SerializeField, Tooltip("Probability adjustments for appearance")]
    private GeneEffect[] geneEffects;

    [SerializeField, Range(1, 50), Tooltip("Range for the level of the creature")]
    private Vector2Int levelRange;

    // move this to the combat initializer script
    private void Awake()
    {
        CreatureEgg instantiatedEgg = Instantiate(egg);
        instantiatedEgg.InitializeSingle(geneEffects);
        ChaosCreature creature = instantiatedEgg.Hatch();
        int level = Random.Range(levelRange.x, levelRange.y);
        creature.AssignLevel(level);
    }
}
