using Assets.Scripts.Creatures.Combat;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public enum TraitRarity
    {
        dominant,
        recessive
    }

    [System.Serializable]
    public class Trait
    {
        public TraitRarity rarity;
        public int baseColorIndex;
        public int accentColorIndex;
        public int patternIndex;
    }

    [System.Serializable]
    public class GeneColor
    {
        public string title;
        public string hexcode;
        public int chance;
    }

    [System.Serializable]
    public class Pattern
    {
        public string title;
        public TraitRarity rarity;
        public string spriteTitle;
        public int chance;
    }

    [System.Serializable]
    public class Feature
    {
        public string title;
        public TraitRarity rarity;
        // public string lineartSpriteTitle;
        public string baseSpriteTitle;
        public int chance;
    }

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

    [System.Serializable]
    public class CreatureDetails
    {
        public int eyeColorIndex;
        public Trait body;
        public Trait primary;
        public Trait secondary;
        public Trait tertiary;
    }

    public enum GeneLocation
    {
        body,
        primary,
        secondary,
        tertiary
    }

    public enum GeneAspect
    {
        shape,
        mainColor,
        accentColor,
        pattern
    }

    [System.Serializable]
    public class GeneEffect
    {
        public GeneLocation location;
        public GeneAspect geneAspect;
        public string option;
        public float chance;
    }

    /// <summary>
    /// Used to save creatures
    /// </summary>
    [System.Serializable]
    public class SaveableCreature
    {
        public string species;
        public string creatureName;
        public int level;
        public Stats stats;
        public CreatureDetails details;
    }
}
