using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TextMeshProUGUI slotField;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private ItemListVariable masterList;

    private InventoryItem inventoryItem = null;
    private Item itemData = null;
    private InventoryWindow parent;
    private int slotIndex;
    // private bool selected = false;

    public void Initialize(int index, InventoryWindow window, InventoryItem item = null)
    {
        if (item != null)
        {
            inventoryItem = item;
            itemData = masterList.GetItem(inventoryItem.title);
            sprite.sprite = itemData.Sprite;
            sprite.color = itemData.Color;
            amountField.text = inventoryItem.amount.ToString();
        }
        else
        {
            sprite.color = Color.clear;
            amountField.text = "";
        }

        parent = window;
        slotIndex = index;
        slotField.text = (slotIndex + 1).ToString();
    }

    // public void SetSelected(bool state)
    // {
    //     selected = state;
    //     if (selected)
    //     {
    //         sprite.transform.localScale = new Vector3(1.5f, 1.5f, 1);
    //     }
    //     else
    //     {
    //         sprite.transform.localScale = Vector3.one;
    //     }
    // }

    public void Select()
    {
        parent.Select(slotIndex);
    }
}
