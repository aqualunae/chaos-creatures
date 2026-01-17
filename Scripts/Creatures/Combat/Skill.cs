using Assets.Scripts.Creatures.Combat;
using UnityEngine;
using static Item;

[CreateAssetMenu(fileName = "Skill ", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField, Range(-50, 50)]
    private int power;

    [SerializeField, Range(0, 1)]
    private float critical;

    [SerializeField]
    private bool aoe;

    [SerializeField]
    private bool targetSelf;

    [SerializeField]
    private Aspect aspect;

    [SerializeField]
    private StatAffected statAffected;

    [SerializeField, Range(0.1f, 2)]
    private float multiplier;

    [SerializeField, Range(1, 50)]
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
        
    }
}
