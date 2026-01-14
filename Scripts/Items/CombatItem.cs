using UnityEngine;

[CreateAssetMenu(fileName = "Item_Combat_", menuName = "Items/Combat")]
public class CombatItem : Item
{
    [SerializeField]
    private bool aoe;

    [SerializeField]
    private bool targetSelf;

    [SerializeField, Range(-50, 50)]
    private int power;

    [SerializeField]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2)]
    private float multiplier;

    public override bool UseItem()
    {
        return base.UseItem();
    }
}
