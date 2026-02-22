using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;
using static Item;

public class InventoryWindow : MonoBehaviour
{
    [Header("Selected Item Info")]

    [SerializeField]
    protected Image selectedItemSprite;

    [SerializeField]
    protected TextMeshProUGUI selectedItemTitle;

    [SerializeField]
    protected TextMeshProUGUI selectedItemDescription;

    [Header("References")]

    [SerializeField, Tooltip("Object that has an inventory. Generally the player.")]
    protected GameObjectVariable inventoryOwner;

    [SerializeField, Tooltip("Where should slots be placed?")]
    protected GameObject slotContainer;

    [SerializeField, Tooltip("Inventory slot prefab to instantiate.")]
    protected GameObject inventorySlotPrefab;

    [SerializeField, Tooltip("List of items that can be used to obtain usable data from saveable data.")]
    protected ItemListVariable masterList;

    [SerializeField, Tooltip("Text field that tells the player whether move mode is enabled.")]
    private TextMeshProUGUI moveModeLabel;

    [SerializeField, Tooltip("If there are no valid inventory slots, what button should be selected for keyboard navigation?")]
    protected Button selectIfEmpty;

    protected Inventory inventory;
    protected List<InventorySlot> slots;
    protected bool inventoryIsEmpty = true;

    // when enabled, move items instead of using them on click
    private bool moveMode = false;
    
    /// <summary>
    /// Get the inventory and initialize slots.
    /// Set move mode back to false.
    /// </summary>
    protected virtual void OnEnable()
    {
        inventory = inventoryOwner.Value.GetComponent<Inventory>();
        if (slots == null)
        {
            slots = new List<InventorySlot>();
        }
        Initialize();
        moveMode = false;
    }

    /// <summary>
    /// If slots already exist, discard them.
    /// </summary>
    protected virtual void PurgeSlots()
    {
        if (slots != null && slots.Count > 0)
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                slots[i].gameObject.SetActive(false);
                slots.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Create or refresh inventory slots
    /// </summary>
    protected virtual void Initialize()
    {
        PurgeSlots();

        // draw new slots
        for (int i = 0; i < inventory.Size; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, slotContainer.transform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            if (inventory.Items[i] != null) { inventoryIsEmpty = false; }
            slot.Initialize(i, inventory.Items[i]);
            slots.Add(slot);
        }

        if (!inventoryIsEmpty)
        {
            slots[0].GetComponent<Button>().Select();
        }
        else
        {
            VoidSelection();
            selectIfEmpty.Select();
        }
    }

    /// <summary>
    /// Show the details of the selected item in the selected item panel
    /// </summary>
    /// <param name="index">Slot index of the selected inventory slot.</param>
    public virtual void Select(int index)
    {
        if (inventory.Items.Count <= index)
        {
            Debug.Log("Invalid index");
            VoidSelection();
            return;
        }

        InventoryItem selectedItem = inventory.Items[index];
        if (selectedItem != null)
        {
            Item selectedItemData = masterList.GetItem(selectedItem.title);
            selectedItemSprite.sprite = selectedItemData.Sprite;
            selectedItemSprite.color = selectedItemData.Color;
            selectedItemTitle.text = $"{selectedItemData.Title} ({selectedItem.amount})";
            selectedItemDescription.text = selectedItemData.Description;
        }
        else
        {
            VoidSelection();
        }
    }

    /// <summary>
    /// When true, move items instead of using them.
    /// </summary>
    public void ToggleMoveMode()
    {
        moveMode = !moveMode;
        moveModeLabel.text = moveMode ? "Move mode: ON" : "Move mode: OFF";
    }

    /// <summary>
    /// Select an item to move.
    /// </summary>
    /// <param name="index">Slot index of item</param>
    protected virtual void MoveItem(int index)
    {
        inventory.Move(index);
        Initialize();
        slots[index].GetComponent<Button>().Select();
    }

    /// <summary>
    /// Select an item to use, or to move if move mode is enabled.
    /// </summary>
    /// <param name="index">Slot index of item</param>
    public virtual void UseItem(int index)
    {
        if (moveMode)
        {
            MoveItem(index);
            return;    
        }

        InventoryItem selectedItem = inventory.Items[index];
        if (selectedItem != null)
        {
            Item selectedItemData = masterList.GetItem(selectedItem.title);
            Debug.Log(selectedItem.title);
            // ask player to select a creature on which to use the item
            // check if the item is usable, equipment, or not
            // UseItemResult result = selectedItemData.UseItem();
        }
    }

    /// <summary>
    /// When an empty inventory slot is selected, display a message that reflects that.
    /// </summary>
    protected void VoidSelection()
    {
        selectedItemSprite.color = Color.clear;
        selectedItemTitle.text = "";
        selectedItemDescription.text = inventoryIsEmpty ? "You don't have any items yet." : "Select an item to view its details.";
    }

    public virtual void ReduceStackByOne(int index)
    {
        inventory.ReduceStackByOne(index);

        // refresh inventory to display changes
        Initialize();
    }
}
