using UnityEngine;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEditor;
using System.Linq;
using System;

public class ChaosCreature : MonoBehaviour
{
    [Header("Info")]

    [SerializeField]
    protected string creatureSpecies;

    #region Genetics

    [Header("Features")]

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

    [Header("Options")]

    [SerializeField]
    protected bool primaryUsesBodyColor = true;

    [SerializeField]
    protected bool secondaryUsesBodyColor = false;

    [SerializeField]
    protected bool tertiaryUsesBodyColor = false;

    [SerializeField]
    protected CreatureOptions options;

    [Header("Sprites")]

    [SerializeField]
    protected string spritesheetFilename;

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

    private CreatureDetails details;

    public string GetSpecies()
    {
        return creatureSpecies;
    }

    public CreatureDetails GetDetails()
    {
        return details;
    }

    private int[] GeneticsToIntArray(GeneColor[] colorInput)
    {
        int[] value = new int[colorInput.Length];
        for (int i = 0; i < colorInput.Length; i++)
        {
            value[i] = colorInput[i].chance;
        }
        return value;
    }

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

    private int[] GeneticsToIntArray(Feature[] featureInput)
    {
        int[] value = new int[featureInput.Length];
        for (int i = 0; i < featureInput.Length; i++)
        {
            value[i] = featureInput[i].chance;
        }
        return value;
    }

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

    public GeneticOdds GetEffectedOdds(GeneEffect[] effects)
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
            details.primary.rarity == TraitRarity.dominant &&
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
            details.secondary.rarity == TraitRarity.dominant &&
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
            details.tertiary.rarity == TraitRarity.dominant &&
            details.tertiary.patternIndex >= options.tertiaryFeaturePatterns.Length / 2
        )
        {
            details.tertiary.patternIndex = (int)(options.tertiaryFeaturePatterns.Length / 2) - 1;
        }
    }

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

    private void DrawCreature()
    {
        // Assign base and pattern sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sprites/Creatures/{spritesheetFilename}");
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

    #endregion

    #region Combat

    [Header("Combat")]

    [SerializeField, Range(1, 10)]
    private Vector2Int hpGain;

    [SerializeField, Range(1, 10)]
    private Vector2Int attackGain;

    [SerializeField, Range(1, 10)]
    private Vector2Int defenseGain;

    [SerializeField, Range(1, 10)]
    private Vector2Int speedGain;

    [SerializeField, Range(1, 10)]
    private Vector2Int criticalGain;

    private int level;

    private Stats stats;

    public void LevelUp()
    {
        level++;
        stats.hp *= UnityEngine.Random.Range(hpGain.x / 100, hpGain.y / 100) + 1;
        stats.attack *= UnityEngine.Random.Range(attackGain.x / 100, attackGain.y / 100) + 1;
        stats.defense *= UnityEngine.Random.Range(defenseGain.x / 100, defenseGain.y / 100) + 1;
        stats.speed *= UnityEngine.Random.Range(speedGain.x / 100, speedGain.y / 100) + 1;
        stats.critical *= UnityEngine.Random.Range(criticalGain.x / 100, criticalGain.y / 100) + 1;

        Debug.Log(JsonUtility.ToJson(stats).ToString());  
    }

    public void AssignLevel(int levelInput)
    {
        for (int i = level; i <= levelInput; i++)
        {
            LevelUp();
        }
    }

    #endregion
}
