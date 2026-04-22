using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField, Tooltip("Character name, if applicable")]
    private string dialogueSource;

    [SerializeField, Tooltip("Dialogue lines when challenge is not enabled.")]
    private string[] lines;

    [SerializeField, Tooltip("Dialogue lines when challenge is enabled.")]
    private string[] challengeLines;

    [SerializeField, Tooltip("Reference to the dialogue window")]
    private DialogueWindow window;

    [SerializeField]
    private GameObjectVariable opponentRef;

    [SerializeField]
    private GameObject locationAlert;

    private bool challengeEnabled = false;

    public string CharacterName
    {
        get => dialogueSource;
    }

    public void OpenDialogue()
    {
        string[] dialogueLines = lines;

        // if you're talking to this npc with challenge enabled
        if (challengeEnabled)
        {
            // set the opponent ref to this so that the attached party will be set as the opponent when combat starts
            opponentRef.Value = gameObject;

            // switch to the challenge dialogue lines if they exist
            dialogueLines = challengeLines.Length > 0 ? challengeLines : lines;
        }

        window.gameObject.SetActive(true);
        window.Initialize(dialogueLines, dialogueSource, challengeEnabled);
    }

    public void EnableChallenge(bool state)
    {
        challengeEnabled = state;
    }

    public void EnableAlert(bool state)
    {
        locationAlert.SetActive(state);
    }
}
