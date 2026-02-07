using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable inventoryOwner;

    [SerializeField]
    private Image selectedItemSprite;

    [SerializeField]
    private TextMeshProUGUI selectedItemTitle;

    [SerializeField]
    private TextMeshProUGUI selectedItemDescription;

    [SerializeField]
    private GameObject slotContainer;

    [SerializeField]
    private GameObject inventorySlotPrefab;

    [SerializeField]
    private ItemListVariable masterList;

    private Inventory inventory;
    private List<InventorySlot> slots;

    private void OnEnable()
    {
        inventory = inventoryOwner.Value.GetComponent<Inventory>();
        if (slots == null)
        {
            slots = new List<InventorySlot>();
        }
        Initialize();
    }

    /// <summary>
    /// Create or refresh inventory slots
    /// </summary>
    private void Initialize()
    {
        // if slots already exist, discard them
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        // draw new slots
        for (int i = 0; i < inventory.Size; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, slotContainer.transform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slot.Initialize(i, this, inventory.Items[i]);
            slots.Add(slot);
        }
    }

    /// <summary>
    /// Show the details of the selected item in the selected item panel
    /// </summary>
    /// <param name="index">Slot index of the selected inventory slot.</param>
    public void Select(int index)
    {
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
            selectedItemSprite.color = Color.clear;
            selectedItemTitle.text = "";
            selectedItemDescription.text = "Select an item to view its details.";
        }
    }
}
