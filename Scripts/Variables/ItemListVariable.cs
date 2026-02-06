using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Item List ", menuName = "Variables/Item List")]
public class ItemListVariable : Variable<Item[]>
{
    public Item GetItem(string itemTitle)
    {
        return Value.First(item => item.Title == itemTitle);
    }

    public Item GetItem(int index)
    {
        return Value[index];
    }
}
