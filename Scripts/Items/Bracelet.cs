using Assets.Scripts.Creatures;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Bracelet ", menuName = "Items/Bracelet")]
public class Bracelet : Item
{
    public enum BraceletStyle
    {
        Beaded,
        Paracord,
        Thread
    }

    [SerializeField, Range(0.6f, 1), Tooltip("Chance that the creature will be befriended.")]
    private float captureRate;

    [SerializeField, Tooltip("Materials used to create the bracelet.")]
    private BraceletStyle style;

    [SerializeField]
    private ColorTitle baseColor;

    [SerializeField]
    private ColorTitle accentColor;

    /// <summary>
    /// Attempt to befriend a creature using this bracelet.
    /// </summary>
    /// <param name="target">Creature you'd like to befriend.</param>
    /// <returns>Results of the item use.</returns>
    public override UseItemResult UseItem(SaveableCreature target)
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        bool success = rand <= captureRate;
        string log = success ? $"You have befriended the wild { target.species }!" : $"The wild { target.species } spurns your friendship.";

        // set this bracelet as the creature's equipment if successful
        if (success)
        {
            target.equipment = new Equipment()
            {
                braceletStyle = style,
                baseColorTitle = baseColor,
                accentColorTitle = accentColor,
                charms = new string[3]
            };
        }

        return new UseItemResult()
        {
            success = success,
            log = log,
            target = target
        };
    }
}
