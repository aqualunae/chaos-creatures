using System.Collections.Generic;
using Assets.Scripts.Creatures;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Where should slots be placed?")]
    protected GameObject slotContainer;

    [SerializeField, Tooltip("Creature slot prefab to instantiate.")]
    protected GameObject slotPrefab;

    [SerializeField]
    protected GameObjectVariable partyOwner;

    [SerializeField]
    protected CreatureOverviewWindow overviewWindow;

    [SerializeField]
    protected TextMeshProUGUI logField;

    [SerializeField]
    protected GameObject actionMenu;

    [SerializeField]
    protected GameObject main;

    protected Party party;
    protected bool showCombat = true;
    protected List<CreatureSlot> slots;
    protected int selectedIndex = -1;
    protected Button[] actionButtons;

    [SerializeField]
    private TextMeshProUGUI moveModeLabel;

    protected bool moveMode = false;
    protected bool pairMode = false;

    /// <summary>
    /// Decides whether to move a creature or focus the action buttons.
    /// Called when a creature is clicked or focused when an accept key is pressed.
    /// </summary>
    public void OnEnter()
    {
        if (moveMode)
        {
            MoveCreature();
        }
        else if (pairMode)
        {
            PairCreature();
        }
        else
        {
            GoToActions();
        }
    }

    /// <summary>
    /// Toggles move mode.
    /// </summary>
    public void ToggleMoveMode()
    {
        moveMode = !moveMode;
        moveModeLabel.text = moveMode ? "Move Mode: ON" : "Move Mode: OFF";
    }

    protected virtual void OnEnable()
    {
        Initialize();
    }

    /// <summary>
    /// Initializes the party window with creatures from the player's party.
    /// </summary>
    public void Initialize()
    {
        main.SetActive(true);
        overviewWindow?.gameObject.SetActive(false);
        nameField?.gameObject.SetActive(false);

        actionButtons = actionMenu.GetComponentsInChildren<Button>();
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = false;
        }

        if (moveModeLabel != null)
        {
            moveModeLabel.GetComponentInParent<Button>().interactable = true;
            moveMode = false;
            moveModeLabel.text = "Move Mode: OFF";
        }

        selectedIndex = -1;

        party = partyOwner.Value.GetComponent<Party>();
        if (slots == null)
        {
            slots = new List<CreatureSlot>();
        }
        Refresh();
        WriteLog();

        gameObject.SetActive(true);
    }

    /// <summary>
    /// If slots already exist, discard them.
    /// </summary>
    protected void PurgeSlots()
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

    protected virtual Party GetParty()
    {
        return party;
    }

    /// <summary>
    /// Initializes all active creature slots. Called on enable and after changes.
    /// </summary>
    protected virtual void Refresh()
    {
        // purge old slots
        PurgeSlots();

        Dictionary<int, SaveableCreature> creatures = GetParty().Creatures;

        // add party slots
        for (int i = 0; i < creatures.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotContainer.transform);
            CreatureSlot slot = slotObject.GetComponent<CreatureSlot>();
            slots.Add(slot);

            if (creatures[i] != null)
            {
                slot.gameObject.SetActive(true);
                slot.Initialize(creatures[i], i);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }

        if (selectedIndex != -1)
        {
            slots[selectedIndex].Focus();
        }
        else
        {
            main.GetComponent<SelectFirst>().Select();
        }
    }

    /// <summary>
    /// Writes a log to the header displaying the party creature capacity.
    /// </summary>
    protected virtual void WriteLog()
    {
        string count = $"({ party.CreatureCount }/{ party.Creatures.Count })";
        if (logField != null)
        {
            logField.text = $"Your party creatures { count }";
        }
    }

    /// <summary>
    /// Center a creature within the scroll view.
    /// </summary>
    /// <param name="index">Index of slot to center</param>
    public void CenterCreature(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = true;
        }

        SnapTo(slots[index]);
    }

    /// <summary>
    /// View a creature's details.
    /// </summary>
    public void ViewDetails()
    {
        if (selectedIndex == -1)
        {
            Debug.Log("No valid creature selected");
            logField.text = "Select a creature first to view their details.";
            return;
        }

        SaveableCreature creature = GetCreature();

        if (creature != null)
        {
            overviewWindow.Initialize(creature);
            overviewWindow.gameObject.SetActive(true);
            main.SetActive(false);
        }
        else
        {
            logField.text = "Select a creature first to view their details.";
        }
    }

    /// <summary>
    /// Get the currently selected creature.
    /// </summary>
    protected virtual SaveableCreature GetCreature()
    {
        return GetParty().GetByIndex(selectedIndex);
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

    /// <summary>
    /// Focuses the first action button.
    /// </summary>
    public void GoToActions()
    {
        actionButtons[0].Select();
        if (selectedIndex != -1)
        {
            slots[selectedIndex].ToggleSelection(true);
        }
    }

    /// <summary>
    /// Centers the selected creature in the scroll rect.
    /// </summary>
    /// <param name="target">Creature slot to center</param>
    protected void SnapTo(CreatureSlot target)
    {
        Canvas.ForceUpdateCanvases();

        ScrollRect scrollRect = slots[0].GetComponentInParent<ScrollRect>();

        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
        Vector2 childLocalPosition   = target.transform.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            scrollRect.content.localPosition.y
        );

        scrollRect.content.localPosition = result;
    }

    /// <summary>
    /// Tells the party to prepare a creature for swapping slots,
    /// or swap if a creature is already prepared.
    /// </summary>
    public virtual void MoveCreature()
    {
        if (selectedIndex == -1)
        {
            return;
        }
        party.Select(selectedIndex);
        Refresh();
        WriteLog();
    }

    /// <summary>
    /// Tells the party to prepare a creature for pairing,
    /// or execute pairing if a creature is already prepared.
    /// </summary>
    public void PairCreature()
    {
        SaveableCreature result = GetParty().Pair(selectedIndex);
        if (result == null)
        {
            logField.text = "Those creatures aren't compatible.";
            pairMode = false;
        }
        else if (result.Equals(GetCreature()))
        {
            logField.text = $"Pair { result.creatureName } with which other creature?";
            pairMode = true;
        }
        else
        {
            string walkText = "Walk with it to see what's inside!";
            if (party.AddToParty(result) != -1)
            {
                logField.text = $"An egg was added to your party! { walkText }";
            }
            else if (party.AddToStorage(result) != -1)
            {
                logField.text = $"An egg was added to storage! { walkText }";
            }
            else
            {
                logField.text = "You don't have space to pair creatures right now!";
            }
            pairMode = false;
        }

        Refresh();
    }

    protected bool confirmRelease = false;

    [SerializeField]
    protected TextMeshProUGUI releaseField;

    public virtual void ReleaseCreature()
    {
        if (selectedIndex == -1)
        {
            return;
        }

        if (!confirmRelease)
        {
            releaseField.text = $"Release { GetCreature().creatureName }?";
            confirmRelease = true;
            if (selectedIndex != -1)
            {
                slots[selectedIndex].ToggleSelection(true);
            }
        }
        else if (party.CreatureCount == 1)
        {
            logField.text = "You can't release your only party creature.";
            confirmRelease = false;
            releaseField.text = "Release";
        }
        else
        {
            logField.text = $"{ GetCreature().creatureName } has been released to the wild. Goodbye!";
            party.RemoveFromParty(selectedIndex);
            confirmRelease = false;
            releaseField.text = "Release";
            selectedIndex = -1;

            Refresh();
        }
    }

    [SerializeField]
    protected NameField nameField;

    public void DisplayNameField()
    {
        if (selectedIndex != -1 && nameField != null)
        {
            nameField.gameObject.SetActive(true);
            nameField.Initialize(GetCreature(), selectedIndex);
            ToggleActions(false);
        }
    }

    public void DisplayNameField(int index)
    {
        CenterCreature(index);
        nameField.gameObject.SetActive(true);
        nameField.Initialize(GetCreature(), index);
        ToggleActions(false);
    }

    public void ChangeName(int index, string input)
    {
        bool success = GetParty().ChangeName(index, input);
        logField.text = success ? $"Your creature's new name is {input}!" : $"{input} doesn't work well as a name.";
        if (success)
        {
            nameField.gameObject.SetActive(false);
            ToggleActions(true);
            Refresh();
        }
    }

    private void ToggleActions(bool state)
    {
        foreach (Button button in actionButtons)
        {
            button.interactable = state;
        }
    }
}
