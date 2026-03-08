using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;

public class CreatureDetailsWindow : MonoBehaviour
{
    [Header("Fields")]

    [SerializeField]
    private TextMeshProUGUI nameField;
    
    [SerializeField]
    private TextMeshProUGUI speciesField;

    [SerializeField]
    private TextMeshProUGUI rarityField;

    [SerializeField]
    private TextMeshProUGUI eyesField;

    [SerializeField]
    private TextMeshProUGUI bodyPatternField;

    [SerializeField]
    private TextMeshProUGUI bodyColorsField;

    [SerializeField]
    private TextMeshProUGUI primaryFeatureField;

    [SerializeField]
    private TextMeshProUGUI primaryPatternField;

    [SerializeField]
    private TextMeshProUGUI primaryColorsField;

    [SerializeField]
    private TextMeshProUGUI secondaryFeatureField;

    [SerializeField]
    private TextMeshProUGUI secondaryPatternField;

    [SerializeField]
    private TextMeshProUGUI secondaryColorsField;

    [SerializeField]
    private TextMeshProUGUI tertiaryFeatureField;

    [SerializeField]
    private TextMeshProUGUI tertiaryPatternField;

    [SerializeField]
    private TextMeshProUGUI tertiaryColorsField;

    [Header("References")]

    [SerializeField]
    private SpeciesListVariable speciesList;

    public void Initialize(SaveableCreature creature)
    {
        CreatureSpecies species = speciesList.GetSpecies(creature.species);

        nameField.text = creature.creatureName;
        speciesField.text = creature.species;
        rarityField.text = $"{species.GetRarityScore(creature.details)} star";

        eyesField.text = $"{species.GetColorTitle(false, creature.details.eyeColorIndex)} Eyes";
        bodyPatternField.text = species.GetFeatureTitle(GeneLocation.body, FeatureRarity.dominant, creature.details.body.patternIndex);
        bodyColorsField.text = $"{species.GetColorTitle(true, creature.details.body.baseColorIndex)}/{species.GetColorTitle(false, creature.details.body.accentColorIndex)}";

        primaryFeatureField.text = species.GetFeatureTitle(GeneLocation.primary, creature.details.primary.rarity);
        primaryPatternField.text = species.GetFeatureTitle(GeneLocation.primary, creature.details.primary.rarity, creature.details.primary.patternIndex);
        primaryColorsField.text = $"{species.GetColorTitle(true, creature.details.primary.baseColorIndex)}/{species.GetColorTitle(false, creature.details.primary.accentColorIndex)}";

        secondaryFeatureField.text = species.GetFeatureTitle(GeneLocation.secondary, creature.details.secondary.rarity);
        secondaryPatternField.text = species.GetFeatureTitle(GeneLocation.secondary, creature.details.secondary.rarity, creature.details.secondary.patternIndex);
        secondaryColorsField.text = $"{species.GetColorTitle(true, creature.details.secondary.baseColorIndex)}/{species.GetColorTitle(false, creature.details.secondary.accentColorIndex)}";

        tertiaryFeatureField.text = species.GetFeatureTitle(GeneLocation.tertiary, creature.details.tertiary.rarity);
        tertiaryPatternField.text = species.GetFeatureTitle(GeneLocation.tertiary, creature.details.tertiary.rarity, creature.details.tertiary.patternIndex);
        tertiaryColorsField.text = $"{species.GetColorTitle(true, creature.details.tertiary.baseColorIndex)}/{species.GetColorTitle(false, creature.details.tertiary.accentColorIndex)}";
    }
}
