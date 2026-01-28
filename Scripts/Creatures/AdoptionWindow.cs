using Assets.Scripts.Creatures;
using UnityEngine;

public class AdoptionWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject[] slots;

    [SerializeField]
    private RandomCreature[] options;

    [SerializeField]
    private Party playerParty;

    private SaveableCreature[] creatures;

    private void Awake()
    {
        // loop
        // generate creature
        // assign to slot
        // assign click handler
    }

    public void SelectionPrompt(int index)
    {
        // are you sure you want to select this one
    }

    public void ToggleDetails(bool state)
    {
        // switch between combat stats and visual stats
    }

    private void ConfirmSelection(int index)
    {
        // add the selected creature to the player's party
        // close the window
    }
}
