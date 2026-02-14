using Assets.Scripts.Creatures;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSlot : MonoBehaviour
{
    [SerializeField]
    private CreatureRenderer creatureRenderer;

    [SerializeField]
    private CombatStats combatStats;

    [SerializeField]
    private CreatureDetailsWindow visualStats;

    [SerializeField]
    private Button renderTarget;

    [SerializeField]
    private SpeciesListVariable speciesList;

    private int index;

    /// <summary>
    /// Initialize all the components of this slot.
    /// </summary>
    /// <param name="creature">Creature to be shown.</param>
    /// <param name="index">Index of this slot within the window.</param>
    public void Initialize(SaveableCreature creature, int index, bool canSelect = true)
    {
        this.index = index;

        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        creatureRenderer.Initialize(species, creature.details);
        combatStats.Initialize(creature.creatureName, creature.species, creature.level, creature.stats);
        visualStats.Initialize(creature);
        renderTarget.interactable = canSelect;
    }

    /// <summary>
    /// Confirm selection of this slot.
    /// </summary>
    public void Select()
    {
        GetComponentInParent<SelectionListener>().OnSelect(index);
    }

    /// <summary>
    /// Switch view between combat details and visual details.
    /// </summary>
    /// <param name="state">True for combat, false for visual.</param>
    public void ToggleDetails(bool state)
    {
        combatStats.gameObject.SetActive(state);
        visualStats.gameObject.SetActive(!state);
    }
}
