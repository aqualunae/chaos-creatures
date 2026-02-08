using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Creatures;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class CombatInventoryWindow : InventoryWindow
{
    [SerializeField]
    private Button selectIfEmpty;

    [SerializeField]
    private CombatWindow combatWindow;

    private Dictionary<int, InventoryItem> slotContents;

    public CombatWindow GetCombatWindow()
    {
        return combatWindow;
    }

    protected override void OnEnable()
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
    protected override void Initialize()
    {
        PurgeSlots();

        // get only the combat items
        List<InventoryItem> combatItems = new List<InventoryItem>();
        foreach(KeyValuePair<int, InventoryItem> pair in inventory.Items)
        {
            if (pair.Value != null)
            {
                Item item = masterList.GetItem(pair.Value.title);
                if (item.GetType() == typeof(CombatItem) || item.GetType() == typeof(Bracelet))
                {
                    combatItems.Add(pair.Value);
                }
            }
        }

        // draw new slots
        slotContents = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < combatItems.Count; i++)
        {
            slotContents.Add(i, combatItems[i]);
            GameObject slotObject = Instantiate(inventorySlotPrefab, slotContainer.transform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slot.Initialize(i, this, combatItems[i]);
            slots.Add(slot);
        }

        Select(0);
        if (slots != null && slots.Count > 0)
        {
            slots[0].GetComponent<Button>().Select();
        }
    }

    /// <summary>
    /// Show the details of the selected item in the selected item panel
    /// </summary>
    /// <param name="index">Slot index of the selected inventory slot.</param>
    public override void Select(int index)
    {
        if (slotContents.Count <= index)
        {
            Debug.Log("Invalid index");
            selectIfEmpty.Select();
            VoidSelection();
            return;
        }

        InventoryItem selectedItem = slotContents[index];
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

    public override void ReduceStackByOne(int index)
    {
        inventory.RemoveOne(slotContents[index].index);
        Initialize();
    }
}
