using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using UnityEngine;

// Needs to be separate from SaveableCreature because SaveableCreature cannot be instantiated through the inspector and ScriptableObjects don't serialize correctly.
/// <summary>
/// Used to create preset Creatures with fixed stats and appearance.
/// </summary>
[CreateAssetMenu(fileName = "Preset Creature ", menuName = "Scriptable Objects/Creature Instance")]
public class CreatureInstance : ScriptableObject
{
    [SerializeField]
    private string species;

    [SerializeField]
    private string creatureName;

    [SerializeField]
    private int level;

    [SerializeField]
    private Stats stats;

    [SerializeField]
    private CreatureDetails details;

    public CreatureInstance(string species, string creatureName, int level, Stats stats, CreatureDetails details)
    {
        this.species = species;
        this.creatureName = creatureName;
        this.level = level;
        this.stats = stats;
        this.details = details;
    }

    public string Species
    {
        get => species;
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
