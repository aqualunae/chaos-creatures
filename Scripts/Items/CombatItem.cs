using System;
using Assets.Scripts.Creatures;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Combat ", menuName = "Items/Combat")]
public class CombatItem : Item
{
    [SerializeField, Tooltip("Could it target multiple creatures? Not implemented.")]
    private bool aoe;

    [SerializeField, Tooltip("Does this item target your creature instead of your opponent?")]
    private bool targetSelf;

    [SerializeField, Range(-50, 50), Tooltip("Damage is positive, healing is negative.")]
    private int power;

    [SerializeField, Tooltip("Stat to buff or debuff. Not implemented.")]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2), Tooltip("Intensity of stat effect. Not implemented.")]
    private float multiplier;

    public bool TargetSelf
    {
        get => targetSelf;
    }

    public override UseItemResult UseItem(SaveableCreature target)
    {
        string log = "";
        bool success = false;

        // if the item does damage or heals, calculate the amount
        if (power != 0)
        {
            float baseDamage = (float)(power * 0.5f);
            float randomDamage = UnityEngine.Random.Range(baseDamage * 0.8f, baseDamage * 1.2f);
            int finalDamage = (int)Math.Round(randomDamage, 0);

            // make sure the target's hp doesn't exceed boundaries
            target.stats.currentHP -= finalDamage;
            if (target.stats.currentHP < 0)
            {
                target.stats.currentHP = 0;
            }
            else if (target.stats.currentHP > target.stats.hp)
            {
                target.stats.currentHP = (int)target.stats.hp;
            }

            // update the log string
            log += $"{title} was used. ";
            if (finalDamage < 0)
            {
                log += $"{target.creatureName} received {finalDamage * -1} healing. ";
            }
            else
            {
                log += $"{target.creatureName} took {finalDamage} damage. ";
            }

            // register success
            success = true;
        }

        // if the item doesn't do anything, it can't be used
        if (!success)
        {
            log = $"{title} could not be used.";
        }

        // return results
        return new UseItemResult()
        {
            success = success,
            log = log,
            target = target
        };
    }
}
