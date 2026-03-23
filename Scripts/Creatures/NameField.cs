using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;

public class NameField : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private CreatureRenderer creatureRenderer;

    [SerializeField]
    private TextMeshProUGUI titleField;

    [SerializeField]
    private TextMeshProUGUI speciesField;

    [SerializeField]
    private TextMeshProUGUI levelField;

    [SerializeField]
    private TextMeshProUGUI partyLogField;

    [SerializeField]
    private SpeciesListVariable speciesList;

    private int slotIndex;

    public void Initialize(SaveableCreature creature, int slotIndex)
    {
        this.slotIndex = slotIndex;

        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        creatureRenderer.Initialize(species, creature.details);

        partyLogField.text = "Rename your creature";
        inputField.text = "New name...";
        titleField.text = $"Renaming { creature.creatureName }";
        speciesField.text = creature.species;
        levelField.text = $"Lvl { creature.level }";

        inputField.Select();
    }

    public void OnConfirm()
    {
        if (string.IsNullOrEmpty(inputField.text) || inputField.text.Equals("New name..."))
        {
            return;
        }

        string input = inputField.text;
        GetComponentInParent<PartyWindow>().ChangeName(slotIndex, input);
    }
}
