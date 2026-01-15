using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Items/Generic")]
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
