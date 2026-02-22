using Assets.Scripts.Creatures;
using UnityEngine;

[CreateAssetMenu(fileName = "Item ", menuName = "Items/Generic")]
public class Item : ScriptableObject
{
    /// <summary>
    /// When an item is used, return this to the UI for processing and display.
    /// </summary>
    public class UseItemResult
    {
        public bool success;
        public string log;
        public SaveableCreature target;
    }

    /// <summary>
    /// Possible stats that can be changed by items or skills.
    /// </summary>
    public enum StatAffected
    {
        none,
        attack,
        defense,
        speed,
        critical
    }

    [SerializeField, Tooltip("Title of the item. Must be unique. Used in save data.")]
    protected string title;

    [SerializeField, Tooltip("Player-facing description of the item.")]
    protected string description;

    [SerializeField, Tooltip("Icon of the item.")]
    protected Sprite sprite;

    [SerializeField, Tooltip("Color to apply to the item. Set to white if the sprite is pre-colored.")]
    protected ColorTitle color = ColorTitle.white;

    public string Title
    {
        get => title;
    }

    public string Description
    {
        get => description;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    public ColorTitle Color
    {
        get => color;
    }

    /// <summary>
    /// Generic items cannot be used on creatures. This should not be called.
    /// </summary>
    public virtual UseItemResult UseItem(SaveableCreature target)
    {
        return new UseItemResult()
        {   
            success = false,
            log = "Unable to use item.",
            target = target
        };
    }
}
