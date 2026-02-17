using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class ItemStorageWindow : InventoryWindow
{
    [SerializeField]
    private GameObjectVariable storageRef;

    [SerializeField]
    private GameObject storageSlotContainer;

    [SerializeField]
    private GameStateEvent pauzeEvent;

    protected Inventory storage;

    protected override void OnEnable()
    {
        pauzeEvent.Invoke(GameState.StorageWindow);
        storage = storageRef.Value.GetComponent<Inventory>();
        base.OnEnable();
    }

    private void OnDisable()
    {
        pauzeEvent.Invoke(GameState.Overworld);
    }

    /// <summary>
    /// Create or refresh inventory slots
    /// </summary>
    protected override void Initialize()
    {
        // purge slots, draw player slots, select first
        base.Initialize();

        // add storage slots
        for (int i = 0; i < storage.Size; i++)
        {
            int index = i + inventory.Items.Count;
            GameObject slotObject = Instantiate(inventorySlotPrefab, storageSlotContainer.transform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            if (storage.Items[i] != null) { inventoryIsEmpty = false; }
            slot.Initialize(index, storage.Items[i]);
            slots.Add(slot);
        }
    }

    /// <summary>
    /// Show the details of the selected item in the selected item panel
    /// </summary>
    /// <param name="index">Slot index of the selected inventory slot.</param>
    public override void Select(int index)
    {
        InventoryItem selectedItem;

        // if the slot index is higher than the base inventory
        if (inventory.Items.Count <= index)
        {
            // if the slot index is higher than the combined total inventory
            if ((inventory.Items.Count + storage.Items.Count) <= index)
            {
                Debug.Log("Invalid index");
                VoidSelection();
                return;
            }

            // select a storage item
            Debug.Log(index - inventory.Items.Count);
            selectedItem = storage.Items[index - inventory.Items.Count];
        }
        else
        {
            // select an inventory item
            selectedItem = inventory.Items[index];
        }

        // render item details
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

    private int selectedIndex = -1;

    /// <summary>
    /// Select an item to move.
    /// </summary>
    /// <param name="index">Slot index of item</param>
    protected override void MoveItem(int index)
    {
        // if a slot was previously selected
        if (selectedIndex != -1)
        {
            // if both slots are in the player's inventory
            if (selectedIndex < inventory.Items.Count && index < inventory.Items.Count)
            {
                inventory.Move(selectedIndex);
                inventory.Move(index);
                selectedIndex = -1;
            }
            // if both slots are in storage
            else if (selectedIndex >= inventory.Items.Count && index >= inventory.Items.Count)
            {
                storage.Move(selectedIndex - inventory.Items.Count);
                storage.Move(index - inventory.Items.Count);
                selectedIndex = -1;
            }
            // if the first slot is in the player's inventory and the second slot is in storage
            else if (selectedIndex < inventory.Items.Count && index >= inventory.Items.Count)
            {
                InventoryItem playerItem = inventory.Items[selectedIndex];
                InventoryItem storageItem = storage.Items[index - inventory.Items.Count];

                inventory.Items[selectedIndex] = storageItem;
                storage.Items[index - inventory.Items.Count] = playerItem;
            }
            // if the first slot is in storage and the second slot is in the player's inventory
            else if (selectedIndex >= inventory.Items.Count && index < inventory.Items.Count)
            {
                InventoryItem playerItem = inventory.Items[index];
                InventoryItem storageItem = storage.Items[selectedIndex - inventory.Items.Count];

                inventory.Items[index] = storageItem;
                storage.Items[selectedIndex - inventory.Items.Count] = playerItem;
            }

            // reset selectedIndex
            selectedIndex = -1;
        }
        else
        {
            selectedIndex = index;
        }
        
        Initialize();
        slots[index].GetComponent<Button>().Select();
    }

    /// <summary>
    /// Select an item to move.
    /// Storage inventory window is always in move mode
    /// </summary>
    /// <param name="index">Slot index of item</param>
    public override void UseItem(int index)
    {
        MoveItem(index);
    }
}
