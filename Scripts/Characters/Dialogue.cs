using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField, Tooltip("Character name, if applicable")]
    private string dialogueSource;

    [SerializeField, Tooltip("Dialogue lines when challenge is not enabled.")]
    private string[] lines;

    [SerializeField, Tooltip("Dialogue lines when challenge is enabled.")]
    private string[] challengeLines;

    [SerializeField, Tooltip("What does the character say when they win?")]
    private string[] victoryLines;

    [SerializeField, Tooltip("What does the character say when they lose?")]
    private string[] defeatLines;

    [SerializeField, Tooltip("Reference to the dialogue window")]
    private DialogueWindow window;

    [SerializeField]
    private GameObjectVariable opponentRef;

    [SerializeField]
    private GameObject locationAlert;

    private bool challengeEnabled = false;
    private bool showVictoryLines = false;
    private bool showDefeatLines = false;

    public string CharacterName
    {
        get => dialogueSource;
    }

    public void OpenDialogue()
    {
        string[] dialogueLines = lines;

        // set the opponent ref to this so that the attached party will be set as the opponent when combat starts
        if (opponentRef != null && TryGetComponent(out Party party))
        {
            opponentRef.Value = gameObject;
        }

        // if you've already fought this npc, show dialogue lines related to the result of the fight
        if (showDefeatLines)
        {
            dialogueLines = defeatLines.Length > 0 ? defeatLines : lines;
        }
        else if (showVictoryLines)
        {
            dialogueLines = victoryLines.Length > 0 ? victoryLines : lines;
        }
        else if (challengeEnabled)
        {
            // if challenge is enabled and you haven't just fought them
            // switch to the challenge dialogue lines if they exist
            dialogueLines = challengeLines.Length > 0 ? challengeLines : lines;
        }

        window.gameObject.SetActive(true);
        window.Initialize(dialogueLines, dialogueSource, challengeEnabled);
    }

    public void EnableChallenge(bool state)
    {
        Debug.Log(state);
        challengeEnabled = state;
    }

    public void EnableAlert(bool state)
    {
        Debug.Log(state);
        locationAlert.SetActive(state);
    }

    public void ShowVictoryLines(bool state)
    {
        showVictoryLines = state;
    }

    public void ShowDefeatLines(bool state)
    {
        showDefeatLines = state;
    }
}
