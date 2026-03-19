using Assets.Scripts.Creatures;
using TMPro;
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

    [SerializeField]
    private TextMeshProUGUI nameField;

    public void Initialize(SaveableCreature creature)
    {
        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        nameField.text = creature.creatureName;
        creatureRenderer.Initialize(species, creature.details);
        visualStats.Initialize(creature);
        combatStats.Initialize("Combat", creature.species, creature.level, creature.stats);
        equipmentPanel.Initialize(creature.equipment);
    }
}
