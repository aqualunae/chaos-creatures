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

    private void Select()
    {
        window.SelectionPrompt(index);
    }

    private void OnDisable()
    {
        renderTarget.onClick.RemoveListener(Select);
    }

    public void ToggleDetails(bool state)
    {
        combatStats.gameObject.SetActive(state);
        visualStats.gameObject.SetActive(!state);
    }
}
