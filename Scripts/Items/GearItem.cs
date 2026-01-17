using UnityEngine;
using Assets.Scripts.Creatures;

[CreateAssetMenu(fileName = "Item Gear ", menuName = "Items/Gear")]
public class GearItem : Item
{
    [SerializeField]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2)]
    private float multiplier;

    [SerializeField, Range(0, 2), Tooltip("1 for first parent, 2 for second parent, 0 for does not enable breeding")]
    private int parent;

    [SerializeField]
    private GeneEffect[] geneEffects;

    public override bool UseItem()
    {
        return base.UseItem();
    }
}
