using Assets.Scripts.Creatures.Combat;
using UnityEngine;
using static Item;

[CreateAssetMenu(fileName = "Skill ", menuName = "Scriptable Objects/Skill")]
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

    public void UseSkill(ChaosCreature user, ChaosCreature target)
    {
        Debug.Log(title);
    }
}
