using Assets.Scripts.Creatures;
using UnityEngine;

public class CreatureRenderer : MonoBehaviour
{
    [Header("Sprites"), Tooltip("For rendering.")]

    [SerializeField]
    protected SpriteRenderer eyeBase;

    [SerializeField]
    protected SpriteRenderer bodyBase;

    [SerializeField]
    protected SpriteRenderer bodyPattern;

    [SerializeField]
    protected SpriteRenderer primaryBase;

    [SerializeField]
    protected SpriteRenderer primaryPattern;

    [SerializeField]
    protected SpriteRenderer secondaryBase;

    [SerializeField]
    protected SpriteRenderer secondaryPattern;

    [SerializeField]
    protected SpriteRenderer tertiaryBase;

    [SerializeField]
    protected SpriteRenderer tertiaryPattern;

    [SerializeField]
    protected SpriteRenderer eyeShine;

    [SerializeField]
    private Sprite eggSprite;

    private CreatureSpecies species;
    private CreatureDetails details;
    private SpeciesOptions options;

    /// <summary>
    /// Initialize this prefab with the required rendering information.
    /// </summary>
    /// <param name="inSpecies">Species data</param>
    /// <param name="inDetails">Instance data</param>
    public void Initialize(CreatureSpecies inSpecies, CreatureDetails inDetails)
    {
        species = inSpecies;
        details = inDetails;
        options = inSpecies.Options;

        Validate();

        if (details.eggSteps > 0)
        {
            DrawEgg();
        }
        else
        {
            DrawCreature();
        }
    }

    /// <summary>
    /// If indices in the details are higher than the lengths of the arrays they correspond to, set them to the highest available index.
    /// </summary>
    private void Validate()
    {
        if (details.eyeColorIndex >= options.accentColors.Length)
        {
            details.eyeColorIndex = options.accentColors.Length - 1;
        }

        if (details.body.baseColorIndex >= options.baseColors.Length)
        {
            details.body.baseColorIndex = options.baseColors.Length - 1;
        }

        if (details.body.accentColorIndex >= options.accentColors.Length)
        {
            details.body.accentColorIndex = options.accentColors.Length - 1;
        }

        if (details.body.patternIndex >= options.bodyPatterns.Length)
        {
            details.body.patternIndex = options.bodyPatterns.Length - 1;
        }

        if (details.primary.baseColorIndex >= options.baseColors.Length)
        {
            details.primary.baseColorIndex = options.baseColors.Length - 1;
        }

        if (details.primary.accentColorIndex >= options.accentColors.Length)
        {
            details.primary.accentColorIndex = options.accentColors.Length - 1;
        }

        if (details.primary.patternIndex >= options.primaryFeaturePatterns.Length)
        {
            details.primary.patternIndex = options.primaryFeaturePatterns.Length - 1;
        }

        // dominant and recessive trait versions of the pattern are stored as separate entries, dominant first
        if (
            details.primary.rarity == FeatureRarity.dominant &&
            details.primary.patternIndex >= options.primaryFeaturePatterns.Length / 2
        )
        {
            details.primary.patternIndex = (int)(options.primaryFeaturePatterns.Length / 2) - 1;
        }

        if (details.secondary.baseColorIndex >= options.baseColors.Length)
        {
            details.secondary.baseColorIndex = options.baseColors.Length - 1;
        }

        if (details.secondary.accentColorIndex >= options.accentColors.Length)
        {
            details.secondary.accentColorIndex = options.accentColors.Length - 1;
        }

        if (details.secondary.patternIndex >= options.secondaryFeaturePatterns.Length)
        {
            details.secondary.patternIndex = options.secondaryFeaturePatterns.Length - 1;
        }

        if (
            details.secondary.rarity == FeatureRarity.dominant &&
            details.secondary.patternIndex >= options.secondaryFeaturePatterns.Length / 2
        )
        {
            details.secondary.patternIndex = (int)(options.secondaryFeaturePatterns.Length / 2) - 1;
        }

        if (details.tertiary.baseColorIndex >= options.baseColors.Length)
        {
            details.tertiary.baseColorIndex = options.baseColors.Length - 1;
        }

        if (details.tertiary.accentColorIndex >= options.accentColors.Length)
        {
            details.tertiary.accentColorIndex = options.accentColors.Length - 1;
        }

        if (details.tertiary.patternIndex >= options.tertiaryFeaturePatterns.Length)
        {
            details.tertiary.patternIndex = options.tertiaryFeaturePatterns.Length - 1;
        }

        if (
            details.tertiary.rarity == FeatureRarity.dominant &&
            details.tertiary.patternIndex >= options.tertiaryFeaturePatterns.Length / 2
        )
        {
            details.tertiary.patternIndex = (int)(options.tertiaryFeaturePatterns.Length / 2) - 1;
        }
    }

    /// <summary>
    /// Use data from Initialize to apply properties to the sprite renderers.
    /// </summary>
    private void DrawCreature()
    {
        eyeBase.sprite = species.GetSprite(GeneLocation.eyes);
        eyeBase.color = species.GetColor(false, details.eyeColorIndex);
        eyeBase.sortingOrder = species.GetSortOrder(GeneLocation.eyes, true);
        eyeShine.sprite = species.GetSprite(GeneLocation.eyes, FeatureRarity.recessive, 1);
        eyeShine.sortingOrder = species.GetSortOrder(GeneLocation.eyes, false);

        bodyBase.sprite = species.GetSprite(GeneLocation.body);
        bodyBase.color = species.GetColor(true, details.body.baseColorIndex);
        bodyBase.sortingOrder = species.GetSortOrder(GeneLocation.body, true);
        bodyPattern.sprite = species.GetSprite(GeneLocation.body, details.body.rarity, details.body.patternIndex);
        bodyPattern.color = species.GetColor(false, details.body.accentColorIndex);
        bodyPattern.sortingOrder = species.GetSortOrder(GeneLocation.body, false);

        primaryBase.sprite = species.GetSprite(GeneLocation.primary, details.primary.rarity);
        primaryBase.color = species.GetColor(true, details.primary.baseColorIndex);
        primaryBase.sortingOrder = species.GetSortOrder(GeneLocation.primary, true);
        primaryPattern.sprite = species.GetSprite(GeneLocation.primary, details.primary.rarity, details.primary.patternIndex);
        primaryPattern.color = species.GetColor(false, details.primary.accentColorIndex);
        primaryPattern.sortingOrder = species.GetSortOrder(GeneLocation.primary, false);

        secondaryBase.sprite = species.GetSprite(GeneLocation.secondary, details.secondary.rarity);
        secondaryBase.color = species.GetColor(true, details.secondary.baseColorIndex);
        secondaryBase.sortingOrder = species.GetSortOrder(GeneLocation.secondary, true);
        secondaryPattern.sprite = species.GetSprite(GeneLocation.secondary, details.secondary.rarity, details.secondary.patternIndex);
        secondaryPattern.color = species.GetColor(false, details.secondary.accentColorIndex);
        secondaryPattern.sortingOrder = species.GetSortOrder(GeneLocation.secondary, false);

        tertiaryBase.sprite = species.GetSprite(GeneLocation.tertiary, details.tertiary.rarity);
        tertiaryBase.color = species.GetColor(true, details.tertiary.baseColorIndex);
        tertiaryBase.sortingOrder = species.GetSortOrder(GeneLocation.tertiary, true);
        tertiaryPattern.sprite = species.GetSprite(GeneLocation.tertiary, details.tertiary.rarity, details.tertiary.patternIndex);
        tertiaryPattern.color = species.GetColor(false, details.tertiary.accentColorIndex);
        tertiaryPattern.sortingOrder = species.GetSortOrder(GeneLocation.tertiary, false); 
    }

    private void DrawEgg()
    {
        bodyBase.sprite = eggSprite;
        bodyBase.color = species.EggColor;

        eyeBase.color = Color.clear;
        eyeShine.color = Color.clear;
        bodyPattern.color = Color.clear;
        primaryBase.color = Color.clear;
        primaryPattern.color = Color.clear;
        secondaryBase.color = Color.clear;
        secondaryPattern.color = Color.clear;
        tertiaryBase.color = Color.clear;
        tertiaryPattern.color = Color.clear;
    }

    /// <summary>
    /// Flip all sprite renderers on the horizontal axis.
    /// </summary>
    public void FlipFacing()
    {
        eyeBase.flipX = true;
        bodyBase.flipX = true;
        bodyPattern.flipX = true;
        primaryBase.flipX = true;
        primaryPattern.flipX = true;
        secondaryBase.flipX = true;
        secondaryPattern.flipX = true;
        tertiaryBase.flipX = true;
        tertiaryPattern.flipX = true;
        eyeShine.flipX = true;
    }
}
