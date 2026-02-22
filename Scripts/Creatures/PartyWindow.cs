using System.Collections.Generic;
using Assets.Scripts.Creatures;
using Unity.VisualScripting;
using UnityEngine;

public class PartyWindow : MonoBehaviour
{
    [SerializeField]
    protected CreatureSlot[] slots;

    [SerializeField]
    protected GameObjectVariable playerRef;

    [SerializeField]
    protected CreatureOverviewWindow overviewWindow;

    protected Party party;
    protected bool toggle = true;

    /// <summary>
    /// Initializes the party window with creatures from the player's party.
    /// </summary>
    protected void OnEnable()
    {
        party = playerRef.Value.GetComponent<Party>();
        Refresh();

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Initializes all active creature slots. Called on enable and after changes.
    /// </summary>
    protected virtual void Refresh()
    {
        Dictionary<int, SaveableCreature> creatures = party.Creatures;
        for (int i = 0; i < slots.Length; i++)
        {
            if (creatures[i] != null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Initialize(creatures[i], i);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Select a creature.
    /// </summary>
    /// <param name="index">Creature slot selected</param>
    public virtual void Select(int index)
    {
        // prepares it for swapping slots
        // party.Select(index);
        // Refresh();

        SaveableCreature creature = party.GetByIndex(index);
        overviewWindow.Initialize(creature);
        overviewWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
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
