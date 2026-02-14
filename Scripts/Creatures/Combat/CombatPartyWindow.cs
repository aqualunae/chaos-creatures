using System.Collections.Generic;
using Assets.Scripts.Creatures;
using UnityEngine;

public class CombatPartyWindow : PartyWindow
{
    public override void Select(int index)
    {
        CombatWindow combatWindow = GetComponentInParent<CombatWindow>();
        SaveableCreature selectedCreature = party.Creatures[index];
        if (selectedCreature == null)
        {
            Debug.Log("Invalid selection index");
        }
        else if (selectedCreature.stats.currentHP > 0)
        {
            party.Select(0);
            party.Select(index);
            combatWindow.SwitchCreature();
        }
        else
        {
            combatWindow.UpdateLog($"{ selectedCreature.creatureName } isn't ready to fight.");
        }
    }

    protected override void Refresh()
    {
        Dictionary<int, SaveableCreature> creatures = party.Creatures;
        for (int i = 0; i < slots.Length; i++)
        {
            if (creatures[i] != null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Initialize(creatures[i], i, creatures[i].stats.currentHP > 0);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}
