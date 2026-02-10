using Assets.Scripts.Creatures;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Bracelet ", menuName = "Items/Bracelet")]
public class Bracelet : Item
{
    public enum BraceletStyle
    {
        beads,
        paracord,
        floss
    }

    [SerializeField, Range(0.6f, 1)]
    private float captureRate;

    [SerializeField]
    private BraceletStyle style;

    [SerializeField]
    private Color mainColor;

    [SerializeField]
    private Color accentColor;

    public override UseItemResult UseItem(SaveableCreature target)
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        bool success = rand <= captureRate;
        string log = success ? $"You have befriended the wild { target.species }!" : $"The wild { target.species } spurns your friendship.";
        return new UseItemResult()
        {
            success = success,
            log = log,
            target = target
        };
    }
}
