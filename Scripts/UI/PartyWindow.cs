using System.Collections.Generic;
using Assets.Scripts.Creatures;
using UnityEngine;

public class PartyWindow : MonoBehaviour
{
    [SerializeField]
    private CreatureSlot[] slots;

    [SerializeField]
    private Party playerParty;

    private bool toggle = true;

    /// <summary>
    /// Initializes the party window with creatures from the player's party.
    /// </summary>
    private void OnEnable()
    {
        Dictionary<int, SaveableCreature> creatures = playerParty.Creatures;
        for (int i = 0; i < slots.Length; i++)
        {
            if (creatures[i] != null)
            {
                slots[i].Initialize(creatures[i], i, this);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
    }

    public void Select(int index)
    {
        // open sub-menu?
    }

    /// <summary>
    /// Toggles between combat details and visual details.
    /// </summary>
    public void ToggleDetails()
    {
        toggle = !toggle;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ToggleDetails(toggle);
        }
    }
}
