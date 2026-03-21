using Assets.Scripts.Creatures.Combat;
using UnityEditor;
using UnityEngine;
using static Bracelet;

namespace Assets.Scripts.Creatures
{
    /// <summary>
    /// Is the feature (shape) more or less likely to appear?
    /// </summary>
    public enum FeatureRarity
    {
        dominant,
        recessive
    }

    /// <summary>
    /// A body part shape and its rarity for a species.
    /// </summary>
    [System.Serializable]
    public class Feature
    {
        public string title;
        public FeatureRarity rarity;
        public string baseSpriteTitle;
        public Sprite sprite;
        public int chance;
    }

    /// <summary>
    /// A color and its rarity for a species.
    /// </summary>
    [System.Serializable]
    public class GeneColor
    {
        public string title;
        public string hexcode;
        public Color color;
        public int chance;
    }

    /// <summary>
    /// Legacy. A pattern and its rarity for one body part of a species.
    /// </summary>
    [System.Serializable]
    public class Pattern
    {
        public string title;
        public FeatureRarity rarity;
        public string spriteTitle;
        public int chance;
    }

    /// <summary>
    /// Legacy. All possible visual options for a creature.
    /// </summary>
    [System.Serializable]
    public class CreatureOptions
    {
        public GeneColor[] baseColors;
        public GeneColor[] accentColors;
        public Pattern[] bodyPatterns;
        public Pattern[] primaryFeaturePatterns;
        public Pattern[] secondaryFeaturePatterns;
        public Pattern[] tertiaryFeaturePatterns;
    }

    // <summary>
    /// All possible visual genetic options for a species.
    /// </summary>
    [System.Serializable]
    public class SpeciesOptions
    {
        public GeneColor[] baseColors;
        public GeneColor[] accentColors;
        public Feature[] bodyPatterns;
        public Feature[] primaryFeaturePatterns;
        public Feature[] secondaryFeaturePatterns;
        public Feature[] tertiaryFeaturePatterns;
    }

    /// <summary>
    /// The data needed to randomize a creature's appearance, with no text descriptors.
    /// </summary>
    [System.Serializable]
    public class GeneticOdds
    {
        public bool primaryUsesBodyColor;
        public bool secondaryUsesBodyColor;
        public bool tertiaryUsesBodyColor;
        public int[] bodyPatternRarity;
        public int[] eyeColorRarity;
        public int[] bodyBaseColorRarity;
        public int[] bodyAccentColorRarity;
        public int[] primaryTraitRarity;
        public int[] primaryPatternRarity;
        public int[] primaryBaseColorRarity;
        public int[] primaryAccentColorRarity;
        public int[] secondaryTraitRarity;
        public int[] secondaryPatternRarity;
        public int[] secondaryBaseColorRarity;
        public int[] secondaryAccentColorRarity;
        public int[] tertiaryTraitRarity;
        public int[] tertiaryPatternRarity;
        public int[] tertiaryBaseColorRarity;
        public int[] tertiaryAccentColorRarity;
    }

    /// <summary>
    /// The details of one body part belonging to one specific creature.
    /// </summary>
    [System.Serializable]
    public class Trait
    {
        public FeatureRarity rarity;
        public int baseColorIndex;
        public int accentColorIndex;
        public int patternIndex;
    }

    /// <summary>
    /// The visual information about one specific creature.
    /// </summary>
    [System.Serializable]
    public class CreatureDetails
    {
        public int eyeColorIndex;
        public Trait body;
        public Trait primary;
        public Trait secondary;
        public Trait tertiary;
        public int eggSteps = 0;
    }

    /// <summary>
    /// The areas of a creature that can be changed.
    /// </summary>
    public enum GeneLocation
    {
        body,
        primary,
        secondary,
        tertiary,
        eyes
    }

    /// <summary>
    /// The specifics that can be changed on one area of a creature.
    /// </summary>
    public enum GeneAspect
    {
        shape,
        mainColor,
        accentColor,
        pattern
    }

    /// <summary>
    /// Used to increase the likelihood of the specified combination, generally in an array. 
    /// </summary>
    [System.Serializable]
    public class GeneEffect
    {
        public GeneLocation location;
        public GeneAspect geneAspect;
        public string option;
    }

    [System.Serializable]
    public class Equipment
    {
        public BraceletStyle braceletStyle;
        public ColorTitle baseColorTitle;
        public ColorTitle accentColorTitle;
        public string[] charms = new string[3];
    }

    /// <summary>
    /// Used to save specific creatures
    /// </summary>
    [System.Serializable]
    public class SaveableCreature
    {
        public string species;
        public string creatureName;
        public int level;
        public Stats stats;
        public CreatureDetails details;
        public Equipment equipment;
    }
}
