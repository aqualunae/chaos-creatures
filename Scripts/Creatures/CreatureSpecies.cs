using UnityEngine;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "Species ", menuName = "Creatures/Species")]
public class CreatureSpecies : ScriptableObject
{
    [Header("Info")]

    [SerializeField]
    protected string species;

    public string Species
    {
        get => species;
    }

    [Header("Features"), Tooltip("The possible shapes of the species.")]

    [SerializeField]
    protected Sprite body;

    [SerializeField]
    protected Sprite eyes;

    [SerializeField]
    protected Sprite eyeShine;

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
    protected SpeciesOptions options;

    [SerializeField, Tooltip("Used to layer sprites appropriately when rendering creatures of this species. Use each option once.")]
    protected GeneLocation[] renderOrder;

    public SpeciesOptions Options
    {
        get => options;
    }

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

    /// <returns>Array of skills that the species is able to use at the specified level.</returns>
    public Skill[] GetSkills(int level)
    {
        return possibleSkills.Where(skill => skill.MinimumLevel <= level).ToArray();
    }

    // could probably stand to refactor so that patternIndex is before rarity
    /// <summary>
    /// Get a specific sprite for this creature species. Eyeshine is at eyes with a positive patternIndex.
    /// </summary>
    /// <param name="location">Body part</param>
    /// <param name="rarity">Rarity of the feature, if applicable</param>
    /// <param name="patternIndex">Index of the pattern, if applicable. Recessive patterns are expected to be included indices, and not calculated by this method.</param>
    /// <returns>Sprite to render.</returns>
    public Sprite GetSprite(GeneLocation location, FeatureRarity rarity = FeatureRarity.dominant, int patternIndex = -1)
    {
        if (patternIndex == -1)
        {
            switch (location)
            {
                case GeneLocation.body:
                    return body;
                case GeneLocation.eyes:
                    return eyes;
                case GeneLocation.primary:
                    return primary[(int)rarity].sprite;
                case GeneLocation.secondary:
                    return secondary[(int)rarity].sprite;
                case GeneLocation.tertiary:
                    return tertiary[(int)rarity].sprite;
            }
        }
        else
        {
            switch (location)
            {
                case GeneLocation.body:
                    return options.bodyPatterns[patternIndex].sprite;
                case GeneLocation.eyes:
                    return eyeShine;
                case GeneLocation.primary:
                    return options.primaryFeaturePatterns[patternIndex].sprite;
                case GeneLocation.secondary:
                    return options.secondaryFeaturePatterns[patternIndex].sprite;
                case GeneLocation.tertiary:
                    return options.tertiaryFeaturePatterns[patternIndex].sprite;
            }
        }

        Debug.Log("Invalid sprite parameters");
        return null;
    }

    /// <summary>
    /// Get a color this spcecies can be.
    /// </summary>
    /// <param name="isBaseColor">True for base color, false for accent color</param>
    /// <param name="index">Color index</param>
    /// <returns></returns>
    public Color GetColor(bool isBaseColor, int index)
    {
        if (isBaseColor)
        {
            return options.baseColors[index].color;
        }
        else
        {
            return options.accentColors[index].color;
        }
    }

    /// <summary>
    /// Used to layer sprites appropriately when rendering creatures of this species.
    /// </summary>
    /// <param name="location">Body part</param>
    /// <param name="isBase">As opposed to pattern</param>
    public int GetSortOrder(GeneLocation location, bool isBase)
    {
        int order = Array.IndexOf(renderOrder, location) * 2;
        if (!isBase) { order++; }
        return order;
    }

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

    /// <param name="featureInput">An array of named patterns.</param>
    /// <param name="hasRecessive">Are the items in the array duplicated to show dominant and recessive shapes?</param>
    /// <returns>An int array that can be used for randomization.</returns>
    private int[] GeneticsToIntArray(Feature[] featureInput, bool hasRecessive = false)
    {
        int arrayLength = hasRecessive ? featureInput.Length / 2 : featureInput.Length;
        int[] value = new int[arrayLength];
        for (int i = 0; i < arrayLength; i++)
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
    /// Get randomized stats for a level 1 creature of this species.
    /// </summary>
    public Stats GetBaseStats()
    {
        return new Stats()
        {
            hp = UnityEngine.Random.Range(hpGain.x, hpGain.y) * 5,
            attack = UnityEngine.Random.Range(attackGain.x, attackGain.y),
            defense = UnityEngine.Random.Range(defenseGain.x, defenseGain.y),
            speed = UnityEngine.Random.Range(speedGain.x, speedGain.y),
            critical = UnityEngine.Random.Range(criticalGain.x, criticalGain.y)
        };
    }

    /// <summary>
    /// Increase stats by a random amount within species bounds, generally for leveling up.
    /// </summary>
    public Stats IncrementStats(Stats stats)
    {
        stats.hp *= UnityEngine.Random.Range((float)hpGain.x / 100, (float)hpGain.y / 100) + 1;
        stats.attack *= UnityEngine.Random.Range((float)attackGain.x / 100, (float)attackGain.y / 100) + 1;
        stats.defense *= UnityEngine.Random.Range((float)defenseGain.x / 100, (float)defenseGain.y / 100) + 1;
        stats.speed *= UnityEngine.Random.Range((float)speedGain.x / 100, (float)speedGain.y / 100) + 1;
        stats.critical *= UnityEngine.Random.Range((float)criticalGain.x / 100, (float)criticalGain.y / 100) + 1;

        return stats;
    }
}
