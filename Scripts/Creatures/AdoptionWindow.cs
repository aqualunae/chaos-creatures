using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionWindow : MonoBehaviour
{
    [SerializeField]
    private CreatureSlot[] slots;

    [SerializeField]
    private RandomCreature[] options;

    [SerializeField]
    private TextMeshProUGUI log;

    [SerializeField]
    private Button confirm;

    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField, Tooltip("Event called when the game is saved or loaded.")]
    private SaveEvent saveEvent;

    [SerializeField]
    private GameStateEvent pauzeEvent;

    private SaveableCreature[] creatures;
    private bool toggle = true;
    private int selection = -1;

    private void Awake()
    {
        saveEvent.AddListener(AssessSave);
    }

    private void OnDisable()
    {
        saveEvent.RemoveListener(AssessSave);
    }

    /// <summary>
    /// On new game, load the window. On load game, hide it.
    /// </summary>
    /// <param name="state">Represents a change in the game's save state.</param>
    private void AssessSave(SaveState state)
    {
        if (state == SaveState.NewGame)
        {
            Initialize();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Generate and render random creatures for each slot.
    /// </summary>
    private void Initialize()
    {
        // loop
        List<RandomCreature> optionsList = new List<RandomCreature>();
        optionsList.AddRange(options);
        creatures = new SaveableCreature[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            // do not select the same option twice
            int index = UnityEngine.Random.Range(0, optionsList.Count);
            SaveableCreature creature = optionsList[index].GetRandomCreature();
            optionsList.RemoveAt(index);

            creatures[i] = creature;
            slots[i].Initialize(creature, i, this);
        }

        pauzeEvent.Invoke(GameState.OtherMenu);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when the player clicks on a creature's renderer.
    /// </summary>
    /// <param name="index">Index of the slot</param>
    public void Select(int index)
    {
        selection = index;
        log.text = $"Are you sure you wish to select the {creatures[selection].species}?";
        confirm.interactable = true;
        confirm.Select();
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
    /// Adds the creature to the player's party and closes the window.
    /// </summary>
    public void ConfirmSelection()
    {
        // todo: let player name creature

        // add the selected creature to the player's party
        Party playerParty = playerRef.Value.GetComponent<Party>();
        playerParty.AddToParty(creatures[selection]);

        // close the window
        pauzeEvent.Invoke(GameState.Overworld);
        gameObject.SetActive(false);
    }
}
