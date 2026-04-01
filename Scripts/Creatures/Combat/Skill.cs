using System;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEngine;
using static Item;

[CreateAssetMenu(fileName = "Skill ", menuName = "Combat/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField, Tooltip("Skill title.")]
    private string title;

    [SerializeField, Tooltip("Short description of what the skill does.")]
    private string description;

    [SerializeField, Range(-50, 50), Tooltip("Used in damage calculation. Negative numbers heal.")]
    private int power;

    [SerializeField, Range(0, 1), Tooltip("Chance of a critical hit between 0 (never) and 1 (always).")]
    private float critical;

    [SerializeField, Tooltip("Can the skill hit multiple targets?")]
    private bool aoe;

    [SerializeField, Tooltip("Does the skill target the user? Primarily for healing.")]
    private bool targetSelf;

    [SerializeField, Tooltip("Used in damage calculation.")]
    private Aspect aspect;

    [SerializeField, Tooltip("If the skill causes a temporary stat change, which stat?")]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2), Tooltip("Magnitude of the skill's stat change effect.")]
    private float multiplier;

    [SerializeField, Range(1, 50), Tooltip("Minimum level the skill user has to be in order to access this skill.")]
    private int minimumLevel;

    [SerializeField]
    private SkillSprites skillSprites;

    [SerializeField]
    private AudioClip sound;

    public string Title
    {
        get => title;
    }

    public string Description
    {
        get => description;
    }

    public int Power
    {
        get => power;
    }

    public int Critical
    {
        get => (int)(critical * 100);
    }

    public bool AOE
    {
        get => aoe;
    }

    public bool TargetSelf
    {
        get => targetSelf;
    }

    public Aspect Aspect
    {
        get => aspect;
    }

    public int MinimumLevel
    {
        get => minimumLevel;
    }

    public Sprite[] Sprites
    {
        get => skillSprites.Sprites;
    }

    public AudioClip Sound
    {
        get => sound;
    }

    /// <summary>
    /// Use the skill.
    /// </summary>
    /// <param name="user">Creature who is using the skill.</param>
    /// <param name="target">Target of the skill, which may be self.</param>
    /// <param name="logUpdateEvent">Event that can be used to update the log.</param>
    /// <returns>Skill target, with updated health and stats.</returns>
    public SaveableCreature UseSkill(SaveableCreature user, SaveableCreature target, StringEvent logUpdateEvent)
    {
        if (user == target && !targetSelf)
        {
            Debug.Log("This should not be a self-targeting skill.");
            return target;
        }

        if (power < 0 && user != target)
        {
            Debug.Log("This skill heals. Are you sure you meant to use it on your opponent?");
        }

        // initial damage calculations
        float adjustedAttack = user.stats.attack - target.stats.defense;
        float baseDamage = (float)(power * 0.5f * ((adjustedAttack * 0.01) + 1));
        float randomDamage = UnityEngine.Random.Range(baseDamage * 0.8f, baseDamage * 1.2f);

        // critical hit calculations
        float critThreshold = (user.stats.critical * 0.03f) + critical;
        bool criticalHit = UnityEngine.Random.Range(0f, 1f) < critThreshold;
        float damage = criticalHit ? (float)(randomDamage * ((user.stats.critical * 0.1) + 1)) : randomDamage;
        int finalDamage = (int)Math.Round(damage, 0);

        // update target's health
        // finalDamage will be negative if the skill heals
        target.stats.currentHP -= finalDamage;

        // do not allow the target's health to go below 0 or above their maximum
        if (target.stats.currentHP < 0)
        {
            target.stats.currentHP = 0;
        }
        else if (target.stats.currentHP > target.stats.hp)
        {
            target.stats.currentHP = (int)target.stats.hp;
        }

        // todo: skill effects other than damage and healing

        // create the log to display to the player
        string skillEffect = $"{title} was used! ";
        if (finalDamage > 0)
        {
            skillEffect += $"{target.creatureName} took {finalDamage} damage.";
        }
        else if (finalDamage < 0)
        {
            skillEffect += $"{target.creatureName} received {finalDamage * -1} healing.";
        }
        logUpdateEvent.Invoke(skillEffect);

        // return the updated creature
        return target;
    }
}
