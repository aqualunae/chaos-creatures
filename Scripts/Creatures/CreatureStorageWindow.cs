using System.Collections.Generic;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureStorageWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Object that has a party. Generally the player.")]
    private GameObjectVariable partyOwner;

    [SerializeField, Tooltip("Where should slots be placed?")]
    private GameObject slotContainer;

    [SerializeField, Tooltip("Creature slot prefab to instantiate.")]
    private GameObject slotPrefab;

    [SerializeField, Tooltip("Reference to the party that is being opened for storage.")]
    private GameObjectVariable storageRef;

    [SerializeField, Tooltip("Event that lets us pauze the game.")]
    private GameStateEvent pauzeEvent;

    [SerializeField]
    private CreatureOverviewWindow overviewWindow;

    [SerializeField]
    private Button[] actionButtons;

    [SerializeField]
    private TextMeshProUGUI label;

    [SerializeField]
    private GameObject main;

    private Party party;
    private Party storage;
    private List<CreatureSlot> slots;
    private bool viewingParty = false;
    private int selectedIndex = -1;
    private bool showCombat = true;

    /// <summary>
    /// Get party references, create slots list, initialize.
    /// </summary>
    private void OnEnable()
    {
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

    /// <summary>
    /// If slots already exist, discard them.
    /// </summary>
    private void PurgeSlots()
    {
        if (slots != null && slots.Count > 0)
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                slots[i].gameObject.SetActive(false);
                slots.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Draw slots of party or storage.
    /// </summary>
    private void Initialize()
    {
        // purge old slots
        PurgeSlots();

        Party currentView = viewingParty ? party : storage;

        // add party slots
        for (int i = 0; i < currentView.Creatures.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotContainer.transform);
            CreatureSlot slot = slotObject.GetComponent<CreatureSlot>();
            slots.Add(slot);

            if (currentView.Creatures[i] != null)
            {
                slot.gameObject.SetActive(true);
                slot.Initialize(currentView.Creatures[i], i);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
        
        // select first slot
        slots[0].GetComponentInChildren<Button>().Select();
    }

    /// <summary>
    /// Centers the selected creature in the scroll rect.
    /// </summary>
    /// <param name="target">Creature slot to center</param>
    private void SnapTo(CreatureSlot target)
    {
        Canvas.ForceUpdateCanvases();

        ScrollRect scrollRect = slotContainer.GetComponentInParent<ScrollRect>();

        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
        Vector2 childLocalPosition   = target.transform.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            scrollRect.content.localPosition.y
        );

        scrollRect.content.localPosition = result;
    }

    /// <summary>
    /// Set the selected creature by index.
    /// </summary>
    public void Select(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = true;
        }

        SnapTo(slots[index]);
    }

    /// <summary>
    /// Move the creature from party to storage or vice versa.
    /// </summary>
    public void MoveCreature()
    {
        // establish which party you're displaying
        Party currentView = viewingParty ? party : storage;
        Party opposite = viewingParty ? storage : party;

        // get the creature being moved
        SaveableCreature movingCreature = currentView.GetByIndex(selectedIndex);

        // if moving the creature would mean the player's party is empty, don't do it
        if (viewingParty && party.CreatureCount <= 1)
        {
            label.text = "Cannot remove all creatures from party.";
            return;
        }

        // otherwise, if there's space in the target party, move it
        string target = viewingParty ? "storage" : "your party";
        if (opposite.AddToParty(movingCreature))
        {
            currentView.RemoveFromParty(selectedIndex);
            label.text = $"{ movingCreature.creatureName } has been moved to { target }.";
        }
        else
        {
            label.text = $"There's no more room in { target }.";
        }

        // redraw slots
        Initialize();
    }

    /// <summary>
    /// Switch to the creature details window for the selected creature.
    /// </summary>
    public void ViewDetails()
    {
        Party currentView = viewingParty ? party : storage;

        if (selectedIndex == -1)
        {
            Debug.Log("No valid creature selected");
            label.text = "Select a creature first to view their details.";
            return;
        }

        SaveableCreature creature = currentView.GetByIndex(selectedIndex);

        if (creature != null)
        {
            overviewWindow.Initialize(creature);
            overviewWindow.gameObject.SetActive(true);
            main.SetActive(false);
        }
        else
        {
            label.text = "Select a creature first to view their details.";
        }
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

        Party currentView = viewingParty ? party : storage;
        string count = $"({ currentView.CreatureCount }/{ currentView.Creatures.Count })";
        label.text = viewingParty ? $"Your party creatures { count }" : $"Creatures in storage { count }";
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = false;
        }
        Initialize();
    }

    public void GoToActions()
    {
        actionButtons[0].Select();
    }

    /// <summary>
    /// Toggles between combat details and visual details.
    /// </summary>
    public void ToggleDetails()
    {
        showCombat = !showCombat;
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ToggleDetails(showCombat);
        }
    }
}
