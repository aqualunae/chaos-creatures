using Assets.Scripts.Creatures;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionSlot : MonoBehaviour
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
    private AdoptionWindow window;

    /// <summary>
    /// Initialize all the components of this slot and add the selection listener.
    /// </summary>
    /// <param name="creature">Creature to be shown.</param>
    /// <param name="index">Index of this slot within the Adoption Window</param>
    /// <param name="window">Reference to the Adoption Window, for calling methods</param>
    public void Initialize(SaveableCreature creature, int index, AdoptionWindow window)
    {
        this.index = index;
        this.window = window;

        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        creatureRenderer.Initialize(species, creature.details);
        combatStats.Initialize(creature.creatureName, creature.species, creature.level, creature.stats.currentHP, creature.stats.hp);
        visualStats.Initialize(creature);
        renderTarget.onClick.AddListener(Select);
    }

    /// <summary>
    /// Confirm selection of this slot.
    /// </summary>
    private void Select()
    {
        window.SelectionPrompt(index);
    }

    private void OnDisable()
    {
        renderTarget.onClick.RemoveListener(Select);
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
