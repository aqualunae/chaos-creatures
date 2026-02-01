using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField, Tooltip("Character name, if applicable")]
    private string dialogueSource;

    [SerializeField, Tooltip("Dialogue lines")]
    private string[] lines;

    [SerializeField, Tooltip("Reference to the dialogue window")]
    private DialogueWindow window;

    public void OpenDialogue()
    {
        window.gameObject.SetActive(true);
        window.Initialize(lines, dialogueSource);
    }
}
