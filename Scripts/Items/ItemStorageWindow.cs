using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class ItemStorageWindow : InventoryWindow
{
    [SerializeField, Tooltip("Reference to the inventory that is being opened for storage.")]
    private GameObjectVariable storageRef;

    [SerializeField, Tooltip("Where should inventory slots of the storage unit be placed?")]
    private GameObject storageSlotContainer;

    [SerializeField, Tooltip("Event that lets us pauze the game.")]
    private GameStateEvent pauzeEvent;

    protected Inventory storage;
    private int moveIndex;

    /// <summary>
    /// Pauze the game, find the storage inventory, and enable the player's inventory.
    /// </summary>
    protected override void OnEnable()
    {
        pauzeEvent.Invoke(GameState.StorageWindow);
        pauzeEvent.AddListener(PauzeListener);
        storage = storageRef.Value.GetComponent<Inventory>();
        base.OnEnable();
    }

    /// <summary>
    /// Unpauze the game when closing this window.
    /// </summary>
    private void OnDisable()
    {
        pauzeEvent.RemoveListener(PauzeListener);
        pauzeEvent.Invoke(GameState.Overworld);
    }

    private void PauzeListener(GameState state)
    {
        if (state == GameState.Overworld)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Create or refresh inventory slots.
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
                VoidSelection();
                return;
            }

            // select a storage item
            selectedItem = storage.Items[index - inventory.Items.Count];
        }
        else
        {
            // select an inventory item
            selectedItem = inventory.Items[index];
        }

        SnapTo(index);
        selectedIndex = index;
        // for (int i = 0; i < slots.Count; i++)
        // {
        //     slots[i].SetHighlight(i == index);
        // }

        // render item details
        if (selectedItem != null)
        {
            Item selectedItemData = masterList.GetItem(selectedItem.title);
            selectedItemSprite.sprite = selectedItemData.Sprite;
            selectedItemSprite.color = colorList.GetColor(selectedItemData.Color);
            selectedItemTitle.text = $"{selectedItemData.Title} ({selectedItem.amount})";
            selectedItemDescription.text = selectedItemData.Description;
        }
        else
        {
            VoidSelection();
        }
    }

    /// <summary>
    /// Select an item to move.
    /// </summary>
    /// <param name="index">Slot index of item</param>
    protected override void MoveItem(int index)
    {
        // only swap if two different slots were selected
        if (index == moveIndex)
        {
            moveIndex = -1;
            Initialize();
            slots[index].GetComponent<Button>().Select();
            return;
        }

        // if a slot was previously selected
        if (moveIndex != -1)
        {
            // if both slots are in the player's inventory
            if (moveIndex < inventory.Items.Count && index < inventory.Items.Count)
            {
                inventory.Move(moveIndex);
                inventory.Move(index);
                moveIndex = -1;
            }
            // if both slots are in storage
            else if (moveIndex >= inventory.Items.Count && index >= inventory.Items.Count)
            {
                storage.Move(moveIndex - inventory.Items.Count);
                storage.Move(index - inventory.Items.Count);
                moveIndex = -1;
            }
            // if the first slot is in the player's inventory and the second slot is in storage
            else if (moveIndex < inventory.Items.Count && index >= inventory.Items.Count)
            {
                InventoryItem playerItem = inventory.Items[moveIndex];
                InventoryItem storageItem = storage.Items[index - inventory.Items.Count];

                if (playerItem != null && playerItem.title == storageItem?.title)
                {
                    // if the stacks are the same, merge them
                    storage.Items[index - inventory.Items.Count].amount += playerItem.amount;
                    inventory.Items[moveIndex] = null;
                }
                else
                {
                    // otherwise, swap them
                    inventory.Items[moveIndex] = storageItem;
                    storage.Items[index - inventory.Items.Count] = playerItem;
                }
            }
            // if the first slot is in storage and the second slot is in the player's inventory
            else if (moveIndex >= inventory.Items.Count && index < inventory.Items.Count)
            {
                InventoryItem playerItem = inventory.Items[index];
                InventoryItem storageItem = storage.Items[moveIndex - inventory.Items.Count];
                
                if (playerItem != null && playerItem.title == storageItem?.title)
                {
                    // if the stacks are the same, merge them
                    inventory.Items[index].amount += storageItem.amount;
                    storage.Items[moveIndex - inventory.Items.Count] = null;
                }
                else
                {
                    // otherwise, swap them
                    inventory.Items[index] = storageItem;
                    storage.Items[moveIndex - inventory.Items.Count] = playerItem;
                }
            }

            // reset selectedIndex
            moveIndex = -1;

            // after swapping items, refresh the inventory and select the item that was most recently selected
            Initialize();
            slots[index].GetComponent<Button>().Select();
        }
        else
        {
            // if no slot was previously selected, select this slot.
            moveIndex = index;
        }
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

    /// <summary>
    /// Centers the selected item in the scroll rect.
    /// </summary>
    /// <param name="target">Slot to center</param>
    protected override void SnapTo(int index)
    {
        Canvas.ForceUpdateCanvases();
        InventorySlot target = slots[index];

        ScrollRect scrollRect = index < inventory.Items.Count ? slotContainer.GetComponentInParent<ScrollRect>() : storageSlotContainer.GetComponentInParent<ScrollRect>();

        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
        Vector2 childLocalPosition   = target.transform.localPosition;
        Vector2 result = new Vector2(
            scrollRect.content.localPosition.x,
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );

        scrollRect.content.localPosition = result;
    }

    public override void DiscardItem()
    {
        if (selectedIndex == -1)
        {
            return;
        }

        InventoryItem item;
        string log = GetDescriptionEmpty();

        if (inventory.Items.Count <= selectedIndex)
        {
            // if the slot index is higher than the combined total inventory
            if ((inventory.Items.Count + storage.Items.Count) <= selectedIndex)
            {
                VoidSelection();
                return;
            }

            // discard a storage item
            item = storage.Items[selectedIndex - inventory.Items.Count];
            if (item != null)
            {
                log = $"{ item.title } has been discarded.";
                storage.RemoveStack(selectedIndex - inventory.Items.Count);
            }
        }
        else
        {
            // discard an inventory item
            item = inventory.Items[selectedIndex];
            if (item != null)
            {
                log = $"{ item.title } has been discarded.";
                inventory.RemoveStack(selectedIndex);
            }
        }

        selectedIndex = -1;
        Initialize();
        selectedItemDescription.text = log;
    }
}
