using System;
using Assets.Scripts.Creatures;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Combat ", menuName = "Items/Combat")]
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

    public bool TargetSelf
    {
        get => targetSelf;
    }

    public override UseItemResult UseItem(SaveableCreature target)
    {
        string log = "";
        bool success = false;

        if (power != 0)
        {
            float baseDamage = (float)(power * 0.5f);
            float randomDamage = UnityEngine.Random.Range(baseDamage * 0.8f, baseDamage * 1.2f);
            int finalDamage = (int)Math.Round(randomDamage, 0);

            target.stats.currentHP -= finalDamage;
            if (target.stats.currentHP < 0)
            {
                target.stats.currentHP = 0;
            }
            else if (target.stats.currentHP > target.stats.hp)
            {
                target.stats.currentHP = (int)target.stats.hp;
            }

            log += $"{title} was used. ";
            if (finalDamage < 0)
            {
                log += $"{target.creatureName} received {finalDamage * -1} healing. ";
            }
            else
            {
                log += $"{target.creatureName} took {finalDamage} damage. ";
            }
            success = true;
        }

        if (!success)
        {
            log = $"{title} could not be used.";
        }

        return new UseItemResult()
        {
            success = success,
            log = log,
            target = target
        };
    }
}
