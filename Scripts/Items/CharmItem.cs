using UnityEngine;
using Assets.Scripts.Creatures;

[CreateAssetMenu(fileName = "Item Charm ", menuName = "Items/Charm")]
public class CharmItem : Item
{
    [SerializeField]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2)]
    private float multiplier;

    [SerializeField, Range(0, 2), Tooltip("1 for first parent, 2 for second parent, 0 for does not enable breeding")]
    private int parent;

    [SerializeField]
    private GeneEffect[] geneEffects;

    public override UseItemResult UseItem(SaveableCreature target)
    {
        return base.UseItem(target);
    }

    public UseItemResult EquipItem(SaveableCreature target)
    {
        return base.UseItem(target);
    }
}
