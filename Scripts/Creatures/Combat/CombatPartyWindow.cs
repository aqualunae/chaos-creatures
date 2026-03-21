using System.Collections.Generic;
using Assets.Scripts.Creatures;
using UnityEngine;

public class CombatPartyWindow : PartyWindow
{
    /// <summary>
    /// Handle selecting a creature during combat, which sends them out to fight.
    /// </summary>
    /// <param name="index">Slot index of the party creature to send out.</param>
    public void SendToCombat(int index)
    {
        // get the combat window and selected creature
        CombatWindow combatWindow = GetComponentInParent<CombatWindow>();
        SaveableCreature selectedCreature = party.Creatures[index];

        // if the player selects the creature that is currently in combat, go back to the skills menu
        if (index == 0 && selectedCreature.stats.currentHP > 0)
        {
            combatWindow.SelectFirstSkill();
            return;
        }

        // if there is no creature in the selected slot
        if (selectedCreature == null)
        {
            // this should be unreachable, as slots without creatures are not rendered
            Debug.Log("Invalid selection index");
        }
        // if the creature is healthy and ready to fight
        else if (selectedCreature.stats.currentHP > 0 && selectedCreature.level > 0)
        {
            // party.Select() needs to be called twice
            // first to select the first slot
            // and then to swap it with the selected slot
            party.Select(0);
            party.Select(index);

            // once the party has been swapped, notify the combat window
            combatWindow.SwitchCreature();
        }
        else
        {
            // if the selected creature has 0 HP, it cannot fight, and the player needs to be notified
            // this should also be unreachable
            combatWindow.UpdateLog($"{ selectedCreature.creatureName } isn't ready to fight.");
        }
    }

    public void SendToCombat()
    {
        if (selectedIndex != -1)
        {
            SendToCombat(selectedIndex);
        }
    }

    /// <summary>
    /// Rerender all slots.
    /// </summary>
    protected override void Refresh()
    {
        // purge old slots
        PurgeSlots();

        Dictionary<int, SaveableCreature> creatures = party.Creatures;

        // add party slots
        for (int i = 0; i < creatures.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotContainer.transform);
            CreatureSlot slot = slotObject.GetComponent<CreatureSlot>();
            slots.Add(slot);

            if (creatures[i] != null)
            {
                slot.gameObject.SetActive(true);
                // third parameter makes creatures with 0 HP non-selectable
                slot.Initialize(creatures[i], i, creatures[i].stats.currentHP > 0 && creatures[i].level > 0);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }
}
