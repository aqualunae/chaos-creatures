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

    /// <summary>
    /// Draw the inventory slot
    /// </summary>
    /// <param name="index">Index of this slot within the list of slots on the inventory window</param>
    /// <param name="window">The inventory window itself</param>
    /// <param name="item">The item data to display, if the slot is filled</param>
    public void Initialize(int index, InventoryWindow window, InventoryItem item = null)
    {
        // if the item exists, display it
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
            // otherwise, hide the sprite and amount
            sprite.color = Color.clear;
            amountField.text = "";
        }

        // set variables that will allow us to send messages back to the parent window later
        parent = window;
        slotIndex = index;

        // players want their inventory to not be zero-indexed, probably
        slotField.text = (slotIndex + 1).ToString();
    }

    /// <summary>
    /// Tell the inventory window that this slot has been selected.
    /// </summary>
    public void Select()
    {
        parent.Select(slotIndex);
    }

    /// <summary>
    /// Use the item
    /// </summary>
    public void UseItem()
    {
        // if the item is being used during combat
        if (parent is CombatInventoryWindow)
        {
            CombatInventoryWindow window = parent as CombatInventoryWindow;
            CombatWindow combatWindow = window.GetCombatWindow();

            // determine which creature should be targeted by the item
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

            // attempt to use the item
            Item.UseItemResult result = itemData.UseItem(target);
            string log = result.log;

            // if using a bracelet was successful, handle friendship
            if (itemData is Bracelet && result.success)
            {
                if (combatWindow.BefriendCreature(target))
                {
                    log += " It was added to your party.";
                    parent.ReduceStackByOne(slotIndex);
                }
                else
                {
                    log += " But there's no room in your party!";
                    combatWindow.TogglePlayerTurn(false);
                }
                combatWindow.UpdateLog(log);
                return;
            }
            // if using a bracelet was unsuccessful, the player's turn is over
            else if (itemData is Bracelet && !result.success)
            {
                parent.ReduceStackByOne(slotIndex);
                combatWindow.UpdateLog(log);
                combatWindow.TogglePlayerTurn(false);
            }
            // if a combat item was used successfully, update the target and end the player's turn
            else if (result.success)
            {
                combatWindow.UpdateLog(log);
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
            // if an attempt was made to use a combat item and it was unsuccessful
            // that's probably a code error, so log the result
            // but don't reduce the item stack or end the player's turn
            else
            {
                combatWindow.UpdateLog(log);
            }
        }
    }
}
