using System.Linq;
using Assets.Scripts.Creatures;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureEgg : MonoBehaviour
{
    [SerializeField]
    private ChaosCreature creature;

    private int eyeColorIndex;
    private Trait body;
    private Trait primary;
    private Trait secondary;
    private Trait tertiary;

    public string GetSpecies()
    {
        return creature.GetSpecies();
    }

    /// <summary>
    /// Initializes a Creature Egg with base probability.
    /// </summary>
    public void InitializeDefault()
    {
        Initialize(creature.GetGeneticOdds());
    }

    public void InitializeSingle(GeneEffect[] effects)
    {
        GeneticOdds odds = creature.GetEffectedOdds(effects);

        Initialize(odds);
    }

    /// <summary>
    /// Initializes a Creature Egg that is more likely to look like its parents.
    /// </summary>
    public void InitializePair(ChaosCreature firstParent, ChaosCreature secondParent)
    {
        if (!firstParent.GetSpecies().Equals(secondParent.GetSpecies()))
        {
            Debug.Log("Mismatched Creatures");
            return;
        }

        GeneticOdds defaultOdds = firstParent.GetGeneticOdds();
        CreatureDetails first = firstParent.GetDetails();
        CreatureDetails second = secondParent.GetDetails();

        GeneticOdds adjustedOdds = new GeneticOdds()
        {
            primaryUsesBodyColor = defaultOdds.primaryUsesBodyColor,
            secondaryUsesBodyColor = defaultOdds.secondaryUsesBodyColor,
            tertiaryUsesBodyColor = defaultOdds.tertiaryUsesBodyColor,

            bodyPatternRarity = CreatureUtility.AdjustOdds(defaultOdds.bodyPatternRarity, first.body.patternIndex, second.body.patternIndex),
            eyeColorRarity = CreatureUtility.AdjustOdds(defaultOdds.eyeColorRarity, first.eyeColorIndex, second.eyeColorIndex),
            bodyBaseColorRarity = CreatureUtility.AdjustOdds(defaultOdds.bodyBaseColorRarity, first.body.baseColorIndex, second.body.baseColorIndex),
            bodyAccentColorRarity = CreatureUtility.AdjustOdds(defaultOdds.bodyAccentColorRarity, first.body.accentColorIndex, second.body.accentColorIndex),

            primaryTraitRarity = CreatureUtility.AdjustOdds(defaultOdds.primaryTraitRarity, (int)first.primary.rarity, (int)second.primary.rarity),
            primaryPatternRarity = CreatureUtility.AdjustOdds(defaultOdds.primaryPatternRarity, first.primary.patternIndex, second.primary.patternIndex),
            primaryBaseColorRarity = CreatureUtility.AdjustOdds(defaultOdds.primaryBaseColorRarity, first.primary.baseColorIndex, second.primary.baseColorIndex),
            primaryAccentColorRarity = CreatureUtility.AdjustOdds(defaultOdds.primaryAccentColorRarity, first.primary.accentColorIndex, second.primary.accentColorIndex),

            secondaryTraitRarity = CreatureUtility.AdjustOdds(defaultOdds.secondaryTraitRarity, (int)first.secondary.rarity, (int)second.secondary.rarity),
            secondaryPatternRarity = CreatureUtility.AdjustOdds(defaultOdds.secondaryPatternRarity, first.secondary.patternIndex, second.secondary.patternIndex),
            secondaryBaseColorRarity = CreatureUtility.AdjustOdds(defaultOdds.secondaryBaseColorRarity, first.secondary.baseColorIndex, second.secondary.baseColorIndex),
            secondaryAccentColorRarity = CreatureUtility.AdjustOdds(defaultOdds.secondaryAccentColorRarity, first.secondary.accentColorIndex, second.secondary.accentColorIndex),

            tertiaryTraitRarity = CreatureUtility.AdjustOdds(defaultOdds.tertiaryTraitRarity, (int)first.tertiary.rarity, (int)second.tertiary.rarity),
            tertiaryPatternRarity = CreatureUtility.AdjustOdds(defaultOdds.tertiaryPatternRarity, first.tertiary.patternIndex, second.tertiary.patternIndex),
            tertiaryBaseColorRarity = CreatureUtility.AdjustOdds(defaultOdds.tertiaryBaseColorRarity, first.tertiary.baseColorIndex, second.tertiary.baseColorIndex),
            tertiaryAccentColorRarity = CreatureUtility.AdjustOdds(defaultOdds.tertiaryAccentColorRarity, first.tertiary.accentColorIndex, second.tertiary.accentColorIndex),
        };

        Initialize(adjustedOdds);
    }

    private void Initialize(GeneticOdds odds)
    {
        eyeColorIndex = WeightedRandom(odds.eyeColorRarity);

        int bodyTraitIndex = 0;
        int bodyPatternIndex = WeightedRandom(odds.bodyPatternRarity);
        int bodyBaseIndex = WeightedRandom(odds.bodyBaseColorRarity);
        int bodyAccentIndex = WeightedRandom(odds.bodyAccentColorRarity);

        body = new Trait()
        {
            rarity = (TraitRarity)bodyTraitIndex,
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

        primary = new Trait()
        {
            rarity = (TraitRarity)primaryTraitIndex,
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

        secondary = new Trait()
        {
            rarity = (TraitRarity)secondaryTraitIndex,
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

        tertiary = new Trait()
        {
            rarity = (TraitRarity)tertiaryTraitIndex,
            baseColorIndex = odds.tertiaryUsesBodyColor ? bodyBaseIndex : tertiaryBaseIndex,
            accentColorIndex = tertiaryAccentIndex,
            patternIndex = tertiaryPatternIndex
        };

        Debug.Log(JsonUtility.ToJson(tertiary).ToString());  
    }

    /// <summary>
    /// Generates a random number and checks it against an array of thresholds.
    /// </summary>
    /// <param name="thresholds">An array of probabilities. Should be already sorted from greatest numbers to smallest.</param>
    /// <returns>The index of the threshold that contains the number generated.</returns>
    private int WeightedRandom(int[] thresholds)
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

    public ChaosCreature Hatch()
    {
        gameObject.SetActive(false);
        ChaosCreature instantiatedCreature = Instantiate(creature);
        instantiatedCreature.Initialize(eyeColorIndex, body, primary, secondary, tertiary);
        instantiatedCreature.transform.localScale = Vector2.one;
        instantiatedCreature.transform.position = new Vector2(0.48f, -0.32f);
        return instantiatedCreature;
    }
}
