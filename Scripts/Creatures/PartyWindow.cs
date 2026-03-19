using System.Collections.Generic;
using Assets.Scripts.Creatures;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyWindow : MonoBehaviour
{
    [SerializeField]
    protected CreatureSlot[] slots;

    [SerializeField]
    protected GameObjectVariable playerRef;

    [SerializeField]
    protected CreatureOverviewWindow overviewWindow;

    [SerializeField]
    protected TextMeshProUGUI logField;

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
        SnapTo(slots[index]);
    }

    /// <summary>
    /// View a creature's details.
    /// </summary>
    /// <param name="index">Creature slot selected</param>
    public void ViewDetails(int index)
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

    /// <summary>
    /// Centers the selected creature in the scroll rect.
    /// </summary>
    /// <param name="target">Creature slot to center</param>
    private void SnapTo(CreatureSlot target)
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
}
