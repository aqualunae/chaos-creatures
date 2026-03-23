using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI nameField;

    [SerializeField]
    protected TextMeshProUGUI dialogueField;

    [SerializeField]
    protected Button nextButton;

    [SerializeField]
    protected GameStateEvent pauzeEvent;

    [SerializeField]
    protected Button challengeButton;

    protected string[] dialogueLines;
    protected int currentLine;

    public void Initialize(string[] lines, string source = null, bool enableChallenge = false)
    {
        // turn off the plaque that renders the name field if there is no name
        nameField.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(source));

        challengeButton.gameObject.SetActive(enableChallenge);

        // assign text to fields
        nameField.text = source;
        dialogueField.text = lines[0];

        // assign variables that will let us progress the dialogue
        dialogueLines = lines;
        currentLine = 0;

        // pauze the game
        pauzeEvent.Invoke(GameState.DialogueWindow);
        pauzeEvent.AddListener(Escape);
    }

    public virtual void Next()
    {
        // show the next line of dialogue
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueField.text = dialogueLines[currentLine];
        }
        else
        {
            // if you're at the end, close the window
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        // unpauze
        pauzeEvent.RemoveListener(Escape);
        pauzeEvent.Invoke(GameState.Overworld);
    }

    public void Escape(GameState state)
    {
        // if the pauze key is pressed during dialogue, close the dialogue window
        if (state == GameState.Overworld)
        {
            gameObject.SetActive(false);
        }
    }
}
