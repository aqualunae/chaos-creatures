using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionWindow : MonoBehaviour
{
    [SerializeField]
    private AdoptionSlot[] slots;

    [SerializeField]
    private RandomCreature[] options;

    [SerializeField]
    private TextMeshProUGUI log;

    [SerializeField]
    private Button confirm;

    [SerializeField]
    private Party playerParty;

    [SerializeField]
    private StringEvent progression;

    private SaveableCreature[] creatures;
    private bool toggle = true;
    private int selection = -1;

    private void Awake()
    {
        progression.AddListener(AssessProgression);
    }

    private void OnDisable()
    {
        progression.RemoveListener(AssessProgression);
    }

    private void AssessProgression(string trigger)
    {
        if (trigger == "New Game")
        {
            Initialize();
        }
        else if (trigger == "Load Game")
        {
            gameObject.SetActive(false);
        }
    }

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

        gameObject.SetActive(true);
    }

    public void SelectionPrompt(int index)
    {
        selection = index;
        log.text = $"Are you sure you wish to select the {creatures[selection].species}?";
        confirm.interactable = true;
    }

    public void ToggleDetails()
    {
        toggle = !toggle;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ToggleDetails(toggle);
        }
    }

    public void ConfirmSelection()
    {
        // add the selected creature to the player's party
        playerParty.AddToParty(creatures[selection]);

        // close the window
        gameObject.SetActive(false);
    }
}
