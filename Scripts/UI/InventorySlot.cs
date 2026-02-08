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

    private InventoryItem inventoryItem = null;
    private Item itemData = null;
    private InventoryWindow parent;
    private int slotIndex;

    public void Initialize(int index, InventoryWindow window, InventoryItem item = null)
    {
        if (item != null)
        {
            inventoryItem = item;
            itemData = masterList.GetItem(inventoryItem.title);
            sprite.sprite = itemData.Sprite;
            sprite.color = itemData.Color;
            amountField.text = inventoryItem.amount.ToString();
        }
        else
        {
            sprite.color = Color.clear;
            amountField.text = "";
        }

        parent = window;
        slotIndex = index;
        slotField.text = (slotIndex + 1).ToString();
    }

    public void Select()
    {
        parent.Select(slotIndex);
    }

    public void UseItem()
    {
        if (parent is CombatInventoryWindow)
        {
            CombatInventoryWindow window = parent as CombatInventoryWindow;
            CombatWindow combatWindow = window.GetCombatWindow();
            SaveableCreature target = combatWindow.GetOpponent();
            bool targetSelf = false;
            if (itemData is CombatItem)
            {
                CombatItem combatItem = itemData as CombatItem;
                if (combatItem.TargetSelf)
                {
                    target = combatWindow.GetPlayer();
                    targetSelf = true;
                }
            }
            Item.UseItemResult result = itemData.UseItem(target);
            combatWindow.UpdateLog(result.log);
            if (result.success)
            {
                if (targetSelf)
                {
                    combatWindow.UpdatePlayer(result.target);
                }
                else
                {
                    combatWindow.UpdateOpponent(result.target);
                }
                combatWindow.TogglePlayerTurn(false);
                parent.ReduceStackByOne(slotIndex);
            }
        }
    }
}
