using UnityEngine;

[CreateAssetMenu(fileName = "Item ", menuName = "Items/Generic")]
public class Item : ScriptableObject
{
    // public enum ItemCategory
    // {
    //     charm,
    //     bracelet,
    //     combat,
    //     material,
    //     key
    // }

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

    public Color Color
    {
        get => color;
    }

    public virtual bool UseItem()
    {
        return false;
    }
}
