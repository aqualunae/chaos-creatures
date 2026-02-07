using UnityEngine;

public class LootableItem : MonoBehaviour
{
    [SerializeField]
    private LootTable table;

    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private SpriteRenderer lootRenderer;

    private Lootable loot;

    private void Awake()
    {
        // select and render an item
        loot = table.GetLootable();
        lootRenderer.sprite = loot.item.Sprite;
        lootRenderer.color = loot.item.Color;
    }

    /// <summary>
    /// Add the item to the player's inventory.
    /// </summary>
    public void Pickup()
    {
        if (playerRef.Value.GetComponent<Inventory>().AddItem(loot.item, loot.amount))
        {
            gameObject.SetActive(false);
        }
    }
}
