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

    [SerializeField]
    private int size;

    [SerializeField]
    private ItemListVariable masterList;

    private Dictionary<int, InventoryItem> items;

    public bool AddItem(Item item, int amount)
    {
        // https://stackoverflow.com/a/5425052

        Debug.Log(items[0]);
        bool stackExists = items.Where(slot => slot.Value != null).Select(slot => slot.Value.title == item.Title).FirstOrDefault();
        if (stackExists)
        {
            Debug.Log("stack exists");
            InventoryItem existingStack = items.First(slot => slot.Value.title == item.Title).Value;
            existingStack.amount += amount;
            return true;
        }

        bool slotAvailable = items.Select(slot => slot.Value == null).FirstOrDefault();
        Debug.Log(slotAvailable);
        Debug.Log(items[0] == null);
        if (slotAvailable)
        {
            int availableIndex = items.First(slot => slot.Value == null).Key;
            Debug.Log(availableIndex);

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

    public bool RemoveOne(int index)
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

        // saveData.items.RemoveAll(item => item.amount == 0);

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
        items = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            items.Add(i, null);
        }

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
