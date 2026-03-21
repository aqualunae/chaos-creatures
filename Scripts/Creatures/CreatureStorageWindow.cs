using System.Collections.Generic;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureStorageWindow : PartyWindow
{
    [SerializeField, Tooltip("Reference to the party that is being opened for storage.")]
    private GameObjectVariable storageRef;

    [SerializeField, Tooltip("Event that lets us pauze the game.")]
    private GameStateEvent pauzeEvent;

    private Party storage;
    private bool viewingParty = false;

    /// <summary>
    /// Get party references, create slots list, initialize.
    /// </summary>
    protected override void OnEnable()
    {
        actionButtons = actionMenu.GetComponentsInChildren<Button>();

        selectedIndex = -1;
        
        pauzeEvent.Invoke(GameState.StorageWindow);
        storage = storageRef.Value.GetComponent<Party>();

        party = partyOwner.Value.GetComponent<Party>();
        if (slots == null)
        {
            slots = new List<CreatureSlot>();
        }
        SwitchView(false);
    }

    /// <summary>
    /// Unpauze the game when closing this window.
    /// </summary>
    private void OnDisable()
    {
        pauzeEvent.Invoke(GameState.Overworld);
    }

    protected override Party GetParty()
    {
        return viewingParty ? party : storage;
    }

    /// <summary>
    /// Move the creature from party to storage or vice versa.
    /// </summary>
    public override void MoveCreature()
    {
        // establish which party you're displaying
        Party currentView = viewingParty ? party : storage;
        Party opposite = viewingParty ? storage : party;

        // get the creature being moved
        SaveableCreature movingCreature = currentView.GetByIndex(selectedIndex);

        // if moving the creature would mean the player's party is empty, don't do it
        if (viewingParty && party.CreatureCount <= 1)
        {
            logField.text = "Cannot remove all creatures from party.";
            return;
        }

        // otherwise, if there's space in the target party, move it
        string target = viewingParty ? "storage" : "your party";
        if (opposite.AddToParty(movingCreature))
        {
            currentView.RemoveFromParty(selectedIndex);
            logField.text = $"{ movingCreature.creatureName } has been moved to { target }.";
        }
        else
        {
            logField.text = $"There's no more room in { target }.";
        }

        // redraw slots
        Refresh();

        main.GetComponent<SelectFirst>().Select();
    }

    /// <summary>
    /// Get the currently selected creature.
    /// </summary>
    protected override SaveableCreature GetCreature()
    {
        Party currentView = viewingParty ? party : storage;
        return currentView.GetByIndex(selectedIndex);
    }

    /// <summary>
    /// Switch between viewing party and viewing storage.
    /// </summary>
    public void SwitchView()
    {
        SwitchView(!viewingParty);
    }

    private void SwitchView(bool view)
    {
        viewingParty = view;
        selectedIndex = -1;

        Party currentView = viewingParty ? party : storage;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = false;
        }
        Refresh();
        WriteLog();
    }

    /// <summary>
    /// Writes a log to the header displaying which party is being viewed and its creature capacity.
    /// </summary>
    protected override void WriteLog()
    {
        Party currentView = viewingParty ? party : storage;
        string count = $"({ currentView.CreatureCount }/{ currentView.Creatures.Count })";
        logField.text = viewingParty ? $"Your party creatures { count }" : $"Creatures in storage { count }";
    }

    public override void ReleaseCreature()
    {
        if (selectedIndex == -1)
        {
            return;
        }

        Party currentView = viewingParty ? party : storage;

        if (!confirmRelease)
        {
            releaseField.text = $"Release { GetCreature().creatureName }?";
            confirmRelease = true;
        }
        else if (viewingParty && party.CreatureCount == 1)
        {
            logField.text = "You can't release your only party creature.";
            confirmRelease = false;
            releaseField.text = "Release";
        }
        else
        {
            logField.text = $"{ GetCreature().creatureName } has been released to the wild. Goodbye!";
            currentView.RemoveFromParty(selectedIndex);
            confirmRelease = false;
            releaseField.text = "Release";

            Refresh();
        }
    }
}
