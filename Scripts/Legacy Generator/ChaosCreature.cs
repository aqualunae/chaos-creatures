using UnityEngine;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// Contains data and rendering instructions for a species of creature. Not a SaveableBehaviour; use SaveableCreature class for data of specific creatures, and initialize it into this class as needed.
/// </summary>
public class ChaosCreature : MonoBehaviour
{
    [Header("Info")]

    [SerializeField]
    protected string creatureSpecies;

    [Header("Features"), Tooltip("The possible shapes of the species.")]

    [SerializeField]
    protected Feature eyes;

    [SerializeField]
    protected Feature body;

    [SerializeField]
    protected Feature[] primary;

    [SerializeField]
    protected Feature[] secondary;

    [SerializeField]
    protected Feature[] tertiary;

    [Header("Options"), Tooltip("The possible colors and patterns of the species.")]

    [SerializeField]
    protected bool primaryUsesBodyColor = true;

    [SerializeField]
    protected bool secondaryUsesBodyColor = false;

    [SerializeField]
    protected bool tertiaryUsesBodyColor = false;

    [SerializeField]
    protected CreatureOptions options;

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

    // details for one specific creature to be rendered
    private CreatureDetails details;
    private string creatureName;

    public string Species
    {
        get => creatureSpecies;
    }

    public CreatureDetails Details
    {
        get => details;
        set => details = value;
    }

    public string Name
    {
        get => creatureName;
        set => creatureName = value;
    }

    /// <summary>
    /// Preferred way to initialize a creature.
    /// </summary>
    /// <param name="instance">The creature's data</param>
    public void Initialize(CreatureInstance instance)
    {
        if (!instance.Species.Equals(creatureSpecies))
        {
            Debug.Log("Mismatch between instance and prefab creatures.");
            return;
        }
        
        creatureName = instance.CreatureName;
        level = instance.Level;
        stats = instance.Stats;
        details = instance.Details;

        Validate();
        DrawCreature();
    }

    /// <summary>
    /// Used in Hatch for the Generator, may be legacy now.
    /// </summary>
    public void Initialize(int eyeColor, Trait body, Trait primary, Trait secondary, Trait tertiary)
    {
        details.eyeColorIndex = eyeColor;
        details.body = body;
        details.primary = primary;
        details.secondary = secondary;
        details.tertiary = tertiary;

        Validate();
        DrawCreature();
    }

    /// <summary>
    /// Used in the Combat Window, may be legacy now.
    /// </summary>
    public void Initialize()
    {
        if (details == null)
        {
            Debug.Log("No creature details!");
            return;
        }

        Validate();
        DrawCreature();
    }

    #region Genetics & Rendering

    /// <param name="colorInput">An array of named gene colors.</param>
    /// <returns>An int array that can be used for randomization.</returns>
    private int[] GeneticsToIntArray(GeneColor[] colorInput)
    {
        int[] value = new int[colorInput.Length];
        for (int i = 0; i < colorInput.Length; i++)
        {
            value[i] = colorInput[i].chance;
        }
        return value;
    }

    /// <param name="patternInput">An array of named patterns.</param>
    /// <param name="hasRecessive">Are the items in the array duplicated to show dominant and recessive shapes?</param>
    /// <returns>An int array that can be used for randomization.</returns>
    private int[] GeneticsToIntArray(Pattern[] patternInput, bool hasRecessive)
    {
        int arrayLength = hasRecessive ? patternInput.Length / 2 : patternInput.Length;
        int[] value = new int[arrayLength];
        for (int i = 0; i < arrayLength; i++)
        {
            value[i] = patternInput[i].chance;
        }
        return value;
    }

    /// <param name="featureInput">An array of named features (shapes).</param>
    /// <returns>An int array that can be used for randomization.</returns>
    private int[] GeneticsToIntArray(Feature[] featureInput)
    {
        int[] value = new int[featureInput.Length];
        for (int i = 0; i < featureInput.Length; i++)
        {
            value[i] = featureInput[i].chance;
        }
        return value;
    }

    /// <summary>
    /// Get the default genetic odds for this species.
    /// </summary>
    public GeneticOdds GetGeneticOdds()
    {
        int[] defaultBaseColorRarity = GeneticsToIntArray(options.baseColors);
        int[] defaultAccentColorRarity = GeneticsToIntArray(options.accentColors);

        return new GeneticOdds()
        {
            primaryUsesBodyColor = primaryUsesBodyColor,
            secondaryUsesBodyColor = secondaryUsesBodyColor,
            tertiaryUsesBodyColor = tertiaryUsesBodyColor,

            bodyPatternRarity = GeneticsToIntArray(options.bodyPatterns, false),
            eyeColorRarity = defaultAccentColorRarity,
            bodyBaseColorRarity = defaultBaseColorRarity,
            bodyAccentColorRarity = defaultAccentColorRarity,

            primaryTraitRarity = GeneticsToIntArray(primary),
            primaryPatternRarity = GeneticsToIntArray(options.primaryFeaturePatterns, true),
            primaryBaseColorRarity = defaultBaseColorRarity,
            primaryAccentColorRarity = defaultAccentColorRarity,

            secondaryTraitRarity = GeneticsToIntArray(secondary),
            secondaryPatternRarity = GeneticsToIntArray(options.secondaryFeaturePatterns, true),
            secondaryBaseColorRarity = defaultBaseColorRarity,
            secondaryAccentColorRarity = defaultAccentColorRarity,

            tertiaryTraitRarity = GeneticsToIntArray(tertiary),
            tertiaryPatternRarity = GeneticsToIntArray(options.tertiaryFeaturePatterns, true),
            tertiaryBaseColorRarity = defaultBaseColorRarity,
            tertiaryAccentColorRarity = defaultAccentColorRarity,
        };
    }

    /// <summary>
    /// Get the genetic odds for this creature with modifiers.
    /// </summary>
    /// <param name="effects">Modifiers to the appearance probabilities.</param>
    public GeneticOdds GetGeneticOdds(GeneEffect[] effects)
    {
        GeneticOdds odds = GetGeneticOdds();

        foreach (GeneEffect effect in effects)
        {
            if (effect.location == GeneLocation.body)
            {
                if (effect.geneAspect == GeneAspect.pattern)
                {
                    for (int i = 0; i < options.bodyPatterns.Length; i++)
                    {
                        if (options.bodyPatterns[i].title == effect.option)
                        {
                            odds.bodyPatternRarity = CreatureUtility.AdjustOdds(GeneticsToIntArray(options.bodyPatterns, false), i);
                        }
                    }
                }
                else if (effect.geneAspect == GeneAspect.mainColor)
                {
                    for (int i = 0; i < options.baseColors.Length; i++)
                    {
                        if (options.baseColors[i].title == effect.option)
                        {
                            odds.bodyBaseColorRarity = CreatureUtility.AdjustOdds(GeneticsToIntArray(options.baseColors), i);
                        }
                    }
                }
                else if (effect.geneAspect == GeneAspect.accentColor)
                {
                    for (int i = 0; i < options.accentColors.Length; i++)
                    {
                        if (options.accentColors[i].title == effect.option)
                        {
                            odds.bodyAccentColorRarity = CreatureUtility.AdjustOdds(GeneticsToIntArray(options.accentColors), i);
                            odds.eyeColorRarity = CreatureUtility.AdjustOdds(GeneticsToIntArray(options.accentColors), i);
                        }
                    }
                }
            }
            else if (effect.location == GeneLocation.primary)
            {
                // surely this could be refactored
            }
        }

        return odds;
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
    /// Assign all sprites and colors to the renderers.
    /// </summary>
    private void DrawCreature()
    {
        // Assign base and pattern sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sprites/Creatures/{creatureSpecies}");
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == eyes.baseSpriteTitle)
            {
                eyeBase.sprite = sprite;
            }
            else if (sprite.name == body.baseSpriteTitle)
            {
                bodyBase.sprite = sprite;
            }
            else if (sprite.name == primary.First(trait => trait.rarity == details.primary.rarity).baseSpriteTitle)
            {
                primaryBase.sprite = sprite;
            }
            else if (sprite.name == secondary.First(trait => trait.rarity == details.secondary.rarity).baseSpriteTitle)
            {
                secondaryBase.sprite = sprite;
            }
            else if (sprite.name == tertiary.First(trait => trait.rarity == details.tertiary.rarity).baseSpriteTitle)
            {
                tertiaryBase.sprite = sprite;
            }
            else if (sprite.name == options.bodyPatterns[details.body.patternIndex].spriteTitle)
            {
                bodyPattern.sprite = sprite;
            }
            else if (sprite.name == options.primaryFeaturePatterns[details.primary.patternIndex].spriteTitle)
            {
                primaryPattern.sprite = sprite;
            }
            else if (sprite.name == options.secondaryFeaturePatterns[details.secondary.patternIndex].spriteTitle)
            {
                secondaryPattern.sprite = sprite;
            }
            else if (sprite.name == options.tertiaryFeaturePatterns[details.tertiary.patternIndex].spriteTitle)
            {
                tertiaryPattern.sprite = sprite;
            }
        }

        // Assign colors
        ColorUtility.TryParseHtmlString(options.accentColors[details.eyeColorIndex].hexcode, out Color eyeColor);
        eyeBase.color = eyeBase.sprite != null ? eyeColor : Color.clear;

        ColorUtility.TryParseHtmlString(options.baseColors[details.body.baseColorIndex].hexcode, out Color bodyBaseColor);
        bodyBase.color = bodyBase.sprite != null ? bodyBaseColor : Color.clear;
        ColorUtility.TryParseHtmlString(options.accentColors[details.body.accentColorIndex].hexcode, out Color bodyAccentColor);
        bodyPattern.color = bodyPattern.sprite != null ? bodyAccentColor : Color.clear;

        ColorUtility.TryParseHtmlString(options.baseColors[details.primary.baseColorIndex].hexcode, out Color primaryBaseColor);
        primaryBase.color = primaryBase.sprite != null ? primaryBaseColor : Color.clear;
        ColorUtility.TryParseHtmlString(options.accentColors[details.primary.accentColorIndex].hexcode, out Color primaryAccentColor);
        primaryPattern.color = primaryPattern.sprite != null ? primaryAccentColor : Color.clear;

        ColorUtility.TryParseHtmlString(options.baseColors[details.secondary.baseColorIndex].hexcode, out Color secondaryBaseColor);
        secondaryBase.color = secondaryBase.sprite != null ? secondaryBaseColor : Color.clear;
        ColorUtility.TryParseHtmlString(options.accentColors[details.secondary.accentColorIndex].hexcode, out Color secondaryAccentColor);
        secondaryPattern.color = secondaryPattern.sprite != null ? secondaryAccentColor : Color.clear;

        ColorUtility.TryParseHtmlString(options.baseColors[details.tertiary.baseColorIndex].hexcode, out Color tertiaryBaseColor);
        tertiaryBase.color = tertiaryBase.sprite != null ? tertiaryBaseColor : Color.clear;
        ColorUtility.TryParseHtmlString(options.accentColors[details.tertiary.accentColorIndex].hexcode, out Color tertiaryAccentColor);
        tertiaryPattern.color = tertiaryPattern.sprite != null ? tertiaryAccentColor : Color.clear;
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

    #endregion

    #region Combat

    [Header("Combat")]
    [Header("Bounds of randomzied percent increase to stats when leveling up.")]

    [SerializeField, Tooltip("min: 1; max: 10")]
    private Vector2Int hpGain = new Vector2Int(3, 6);

    [SerializeField, Tooltip("min: 1; max: 10")]
    private Vector2Int attackGain = new Vector2Int(3, 6);

    [SerializeField, Tooltip("min: 1; max: 10")]
    private Vector2Int defenseGain = new Vector2Int(3, 6);

    [SerializeField, Tooltip("min: 1; max: 10")]
    private Vector2Int speedGain = new Vector2Int(3, 6);

    [SerializeField, Tooltip("min: 1; max: 10")]
    private Vector2Int criticalGain = new Vector2Int(3, 6);

    [Header("Skills")]
    [SerializeField, Tooltip("The skills that this species is able to learn.")]
    private Skill[] possibleSkills;

    // Data belonging to a specific instance of creature.
    private int level;
    private Stats stats;

    public int Level
    {
        get => level;
    }

    public Stats Stats
    {
        get => stats;
    }

    /// <summary>
    /// Assign stats and level without modifying them.
    /// </summary>
    public void SetStats(Stats inStats, int inLevel)
    {
        stats = inStats;
        level = inLevel;
    }

    /// <returns>Array of skills that the creature is able to use at its current level.</returns>
    public Skill[] GetSkills()
    {
        return possibleSkills.Where(skill => skill.MinimumLevel <= level).ToArray();
    }

    /// <summary>
    /// Increase level by one and stats by a random amount within species bounds.
    /// </summary>
    public void LevelUp()
    {
        level++;
        stats.hp *= UnityEngine.Random.Range((float)hpGain.x / 100, (float)hpGain.y / 100) + 1;
        stats.attack *= UnityEngine.Random.Range((float)attackGain.x / 100, (float)attackGain.y / 100) + 1;
        stats.defense *= UnityEngine.Random.Range((float)defenseGain.x / 100, (float)defenseGain.y / 100) + 1;
        stats.speed *= UnityEngine.Random.Range((float)speedGain.x / 100, (float)speedGain.y / 100) + 1;
        stats.critical *= UnityEngine.Random.Range((float)criticalGain.x / 100, (float)criticalGain.y / 100) + 1; 
    }

    /// <summary>
    /// Assign level and apply level ups for each level above current. Useful for randomly generated creatures.
    /// </summary>
    /// <param name="levelInput">New level of the creature.</param>
    public void AssignLevel(int levelInput)
    {
        for (int i = level; i < levelInput; i++)
        {
            LevelUp();
        }
    }

    #endregion
}
