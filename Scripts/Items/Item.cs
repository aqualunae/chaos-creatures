using UnityEngine;

[CreateAssetMenu(fileName = "Item ", menuName = "Items/Generic")]
public class Item : ScriptableObject
{
    public enum ItemCategory
    {
        charm,
        combat,
        material,
        key
    }

    public enum StatAffected
    {
        none,
        attack,
        defense,
        speed,
        critical
    }

    [SerializeField]
    protected string title;

    [SerializeField]
    protected string description;

    [SerializeField]
    protected Sprite sprite;

    [SerializeField]
    protected Color color;

    public virtual bool UseItem()
    {
        return false;
    }
}
