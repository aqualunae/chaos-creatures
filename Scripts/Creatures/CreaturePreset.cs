using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "Preset Creature ", menuName = "Scriptable Objects/Creature Preset")]
public class CreaturePreset : ScriptableObject
{
    [SerializeField]
    private ChaosCreature creature;

    [SerializeField]
    private string creatureName;

    [SerializeField]
    private int level;

    [SerializeField]
    private Stats stats;

    [SerializeField]
    private CreatureDetails details;

    public ChaosCreature Creature
    {
        get => creature;
    }

    public string CreatureName
    {
        get => creatureName;
    }

    public int Level
    {
        get => level;
    }

    public Stats Stats
    {
        get => stats;
    }

    public CreatureDetails Details
    {
        get => details;
    }
}
