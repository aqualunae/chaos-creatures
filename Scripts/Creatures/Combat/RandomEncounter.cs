using Assets.Scripts.Creatures;
using UnityEngine;

public class RandomEncounter : ScriptableObject
{
    [SerializeField]
    private CreatureEgg egg;

    [SerializeField]
    private GeneEffect[] geneEffects;

    [SerializeField, Range(1, 50)]
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
