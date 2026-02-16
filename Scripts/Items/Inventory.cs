using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : SaveableBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public string title;
        public int amount;
        public int index;
    }

    [SerializeField, Tooltip("Maximum number of item stacks.")]
    private int size;

    private Dictionary<int, InventoryItem> items;

    /// <summary>
    /// Maximum number of item stacks.
    /// </summary>
    public int Size
    {
        get => size;
    }

    public Dictionary<int, InventoryItem> Items
    {
        get => items;
    }

    /// <summary>
    /// Add an item to the inventory, if possible.
    /// </summary>
    /// <param name="item">Item data</param>
    /// <param name="amount">Quantity of item</param>
    /// <returns>True if successful.</returns>
    public bool AddItem(Item item, int amount)
    {
        // check if the item is already in the inventory
        bool stackExists = items.Count(slot => slot.Value?.title == item.Title) > 0;
        if (stackExists)
        {
            // if it is, add it to the stack
            InventoryItem existingStack = items.First(slot => slot.Value.title == item.Title).Value;
            existingStack.amount += amount;
            return true;
        }

        // otherwise, check if there's an available slot
        bool slotAvailable = items.Count(slot => slot.Value == null) > 0;
        if (slotAvailable)
        {
            // add the item to the available slot
            int availableIndex = items.First(slot => slot.Value == null).Key;
            InventoryItem newStack = new InventoryItem()
            {
                title = item.Title,
                amount = amount,
                index = availableIndex
            };

            items[availableIndex] = newStack;
            return true;
        }

        return false;
    }

    private int selectedIndex = -1;

    public void Move(int index)
    {
        if (selectedIndex == -1)
        {
            selectedIndex = index;
        }
        else
        {
            Swap(selectedIndex, index);
            selectedIndex = -1;
        }
    }

    private void Swap(int first, int second)
    {
        InventoryItem firstItem = items[first];
        InventoryItem secondItem = items[second];

        items[first] = secondItem;
        items[second] = firstItem;
    }

    /// <summary>
    /// Remove one of an item, for example if it's used.
    /// </summary>
    /// <param name="index">Slot index of the inventory stack.</param>
    /// <returns>True if successful.</returns>
    public bool ReduceStackByOne(int index)
    {
        if (items[index] != null)
        {
            if (items[index].amount > 1)
            {
                items[index].amount--;
            }
            else
            {
                items[index] = null;
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// Remove all of an item, for example when selling or discarding.
    /// </summary>
    /// <param name="index">Slot index of the inventory stack</param>
    /// <returns>True if successful.</returns>
    public bool RemoveStack(int index)
    {
        if (items[index] != null)
        {
            items[index] = null;
            return true;
        }
        
        return false;
    }

    #region Saving

    public class InventorySaveData
    {
        public List<InventoryItem> items;
    }

    public override void OnNewGame()
    {
        items = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            items.Add(i, null);
        }
    }

    public override Saveable OnSave()
    {
        InventorySaveData saveData = new InventorySaveData()
        {
            items = this.items.Values.ToList()
        };

        // don't save empty slots
        saveData.items.RemoveAll(item => item == null);

        string data = JsonUtility.ToJson(saveData);
        string identifier = $"{typeof(Inventory)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(saveable.data);

        // create the slots
        items = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            items.Add(i, null);
        }

        // fill slots with saved data
        for (int i = 0; i < saveData.items.Count; i++)
        {
            if (saveData.items[i].amount > 0)
            {
                items[saveData.items[i].index] = saveData.items[i];
            }
        }
    }

    #endregion
}
