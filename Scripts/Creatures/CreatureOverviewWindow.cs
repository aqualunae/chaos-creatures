using Assets.Scripts.Creatures;
using UnityEngine;

public class CreatureOverviewWindow : MonoBehaviour
{
    [SerializeField]
    private CreatureRenderer creatureRenderer;

    [SerializeField]
    private CreatureDetailsWindow visualStats;

    [SerializeField]
    private CombatStats combatStats;

    [SerializeField]
    private CreatureEquipmentPanel equipmentPanel;

    [SerializeField]
    private SpeciesListVariable speciesList;

    public void Initialize(SaveableCreature creature)
    {
        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        creatureRenderer.Initialize(species, creature.details);
        visualStats.Initialize(creature);
        combatStats.Initialize("Combat", creature.species, creature.level, creature.stats);
        equipmentPanel.Initialize(creature.equipment);
    }
}
