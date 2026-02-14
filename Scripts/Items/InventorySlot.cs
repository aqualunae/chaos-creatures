using Assets.Scripts.Creatures;
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

    private int slotIndex;
    private SelectionListener itemListener;

    /// <summary>
    /// Draw the inventory slot
    /// </summary>
    /// <param name="index">Index of this slot within the list of slots on the inventory window</param>
    /// <param name="item">The item data to display, if the slot is filled</param>
    public void Initialize(int index, InventoryItem item = null)
    {
        // if the item exists, display it
        if (item != null)
        {
            Item itemData = masterList.GetItem(item.title);
            sprite.sprite = itemData.Sprite;
            sprite.color = itemData.Color;
            amountField.text = item.amount.ToString();
        }
        else
        {
            // otherwise, hide the sprite and amount
            sprite.color = Color.clear;
            amountField.text = "";
        }

        // set variables that will allow us to send messages back to the parent window later
        slotIndex = index;
        itemListener = GetComponentInParent<SelectionListener>();

        // players want their inventory to not be zero-indexed, probably
        slotField.text = (slotIndex + 1).ToString();
    }

    /// <summary>
    /// Tell the inventory window that this slot has been selected.
    /// </summary>
    public void Select()
    {
        itemListener.OnSelect(slotIndex);
    }

    /// <summary>
    /// Tell the inventory window to use this slot's item.
    /// </summary>
    public void UseItem()
    {
        itemListener.OnActivate(slotIndex);
    }
}
