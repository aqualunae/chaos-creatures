using System.Collections.Generic;
using Assets.Scripts.Creatures;
using Unity.VisualScripting;
using UnityEngine;

public class PartyWindow : MonoBehaviour
{
    [SerializeField]
    private CreatureSlot[] slots;

    [SerializeField]
    private GameObjectVariable playerPartyRef;

    private Party party;
    private bool toggle = true;

    /// <summary>
    /// Initializes the party window with creatures from the player's party.
    /// </summary>
    private void OnEnable()
    {
        Debug.Log("enabling");
        party = playerPartyRef.Value.GetComponent<Party>();
        Refresh();

        gameObject.SetActive(true);
    }

    private void Refresh()
    {
        Dictionary<int, SaveableCreature> creatures = party.Creatures;
        for (int i = 0; i < slots.Length; i++)
        {
            if (creatures[i] != null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Initialize(creatures[i], i, this);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void Select(int index)
    {
        party.Select(index);
        Refresh();
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
