using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField]
    protected GameObjectVariable inventoryOwner;

    [SerializeField]
    protected Image selectedItemSprite;

    [SerializeField]
    protected TextMeshProUGUI selectedItemTitle;

    [SerializeField]
    protected TextMeshProUGUI selectedItemDescription;

    [SerializeField]
    protected GameObject slotContainer;

    [SerializeField]
    protected GameObject inventorySlotPrefab;

    [SerializeField]
    protected ItemListVariable masterList;

    protected Inventory inventory;
    protected List<InventorySlot> slots;

    protected virtual void OnEnable()
    {
        inventory = inventoryOwner.Value.GetComponent<Inventory>();
        if (slots == null)
        {
            slots = new List<InventorySlot>();
        }
        Initialize();
    }

    /// <summary>
    /// If slots already exist, discard them.
    /// </summary>
    protected void PurgeSlots()
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
            slot.Initialize(i, this, inventory.Items[i]);
            slots.Add(slot);
        }

        if (slots != null && slots.Count > 0)
        {
            slots[0].Select();
            Select(0);
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

    protected void VoidSelection()
    {
        selectedItemSprite.color = Color.clear;
        selectedItemTitle.text = "";
        selectedItemDescription.text = "Select an item to view its details.";
    }

    public virtual void ReduceStackByOne(int index)
    {
        inventory.RemoveOne(index);
        Initialize();
    }
}
