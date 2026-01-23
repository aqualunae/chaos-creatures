using UnityEngine;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using System.Linq;

public static class CreatureUtility
{
    /// <summary>
    /// Bias the odds in favor of one or two indices. If the indices match, the effect is increased.
    /// </summary>
    public static int[] AdjustOdds(int[] defaultOdds, int firstIndex, int secondIndex = -1)
    {
        int[] adjustedOdds = new int[defaultOdds.Length];
        for (int i = 0; i < defaultOdds.Length; i++)
        {
            if (i == firstIndex && i == secondIndex)
            {
                adjustedOdds[i] = defaultOdds[i] * 5;
            }
            else if (i == firstIndex || i == secondIndex)
            {
                adjustedOdds[i] = defaultOdds[i] * 2;
            }
            else
            {
                adjustedOdds[i] = (int)(defaultOdds[i] * 0.6);
            }
        }
        return adjustedOdds;
    }

    /// <summary>
    /// Generates a random number and checks it against an array of thresholds.
    /// </summary>
    /// <param name="thresholds">An array of probabilities. Should be already sorted from greatest numbers to smallest.</param>
    /// <returns>The index of the threshold that contains the number generated.</returns>
    public static int WeightedRandom(int[] thresholds)
    {
        int randomMax = thresholds.Sum() + 1;
        int randomNumber = Random.Range(1, randomMax);
        int sum = 0;
        for (int i = 0; i < thresholds.Length; i++)
        {
            sum += thresholds[i];
            if (randomNumber <= sum)
            {
                return i;
            }
        }
        return 0;
    }

    /// <summary>
    /// Get the genetic details of a creature based on given odds.
    /// </summary>
    /// <param name="odds">Chance of each possibility</param>
    public static CreatureDetails GetDetails(GeneticOdds odds)
    {
        int eyeColorIndex = WeightedRandom(odds.eyeColorRarity);

        int bodyTraitIndex = 0;
        int bodyPatternIndex = WeightedRandom(odds.bodyPatternRarity);
        int bodyBaseIndex = WeightedRandom(odds.bodyBaseColorRarity);
        int bodyAccentIndex = WeightedRandom(odds.bodyAccentColorRarity);

        Trait body = new Trait()
        {
            rarity = (FeatureRarity)bodyTraitIndex,
            baseColorIndex = bodyBaseIndex,
            accentColorIndex = bodyAccentIndex,
            patternIndex = bodyPatternIndex
        };

        int primaryBaseIndex = WeightedRandom(odds.primaryBaseColorRarity);
        int primaryAccentIndex = WeightedRandom(odds.primaryAccentColorRarity);
        int primaryTraitIndex = WeightedRandom(odds.primaryTraitRarity);
        int primaryPatternIndex = WeightedRandom(odds.primaryPatternRarity);

        // dominant and recessive trait versions of the pattern are stored as separate entries, dominant first
        if (primaryTraitIndex > 0)
        {
            int patternCount = odds.primaryPatternRarity.Length;
            primaryPatternIndex += patternCount;
        }

        Trait primary = new Trait()
        {
            rarity = (FeatureRarity)primaryTraitIndex,
            baseColorIndex = odds.primaryUsesBodyColor ? bodyBaseIndex : primaryBaseIndex,
            accentColorIndex = primaryAccentIndex,
            patternIndex = primaryPatternIndex
        };
        
        int secondaryTraitIndex = WeightedRandom(odds.secondaryTraitRarity);
        int secondaryPatternIndex = WeightedRandom(odds.secondaryPatternRarity);
        int secondaryBaseIndex = WeightedRandom(odds.secondaryBaseColorRarity);
        int secondaryAccentIndex = WeightedRandom(odds.secondaryAccentColorRarity);

        if (secondaryTraitIndex > 0)
        {
            int patternCount = odds.secondaryPatternRarity.Length;
            secondaryPatternIndex += patternCount;
        }

        Trait secondary = new Trait()
        {
            rarity = (FeatureRarity)secondaryTraitIndex,
            baseColorIndex = odds.secondaryUsesBodyColor ? bodyBaseIndex : secondaryBaseIndex,
            accentColorIndex = secondaryAccentIndex,
            patternIndex = secondaryPatternIndex
        };

        int tertiaryTraitIndex = WeightedRandom(odds.tertiaryTraitRarity);
        int tertiaryPatternIndex = WeightedRandom(odds.tertiaryPatternRarity);
        int tertiaryBaseIndex = WeightedRandom(odds.tertiaryBaseColorRarity);
        int tertiaryAccentIndex = WeightedRandom(odds.tertiaryAccentColorRarity);

        if (tertiaryTraitIndex > 0)
        {
            int patternCount = odds.tertiaryPatternRarity.Length;
            tertiaryPatternIndex += patternCount;
        }

        Trait tertiary = new Trait()
        {
            rarity = (FeatureRarity)tertiaryTraitIndex,
            baseColorIndex = odds.tertiaryUsesBodyColor ? bodyBaseIndex : tertiaryBaseIndex,
            accentColorIndex = tertiaryAccentIndex,
            patternIndex = tertiaryPatternIndex
        };

        return new CreatureDetails()
        {
            eyeColorIndex = eyeColorIndex,
            body = body,
            primary = primary,
            secondary = secondary,
            tertiary = tertiary
        };
    }
}
