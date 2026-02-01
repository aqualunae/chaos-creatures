using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameField;

    [SerializeField]
    private TextMeshProUGUI dialogueField;

    [SerializeField]
    private Button nextButton;

    private string[] dialogueLines;
    private int currentLine;

    public void Initialize(string[] lines, string source = null)
    {
        // turn off the plaque that renders the name field if there is no name
        nameField.transform.parent.gameObject.SetActive(source != null);

        // assign text to fields
        nameField.text = source;
        dialogueField.text = lines[0];

        // assign variables that will let us progress the dialogue
        dialogueLines = lines;
        currentLine = 0;
        nextButton.onClick.AddListener(Next);
    }

    public void Next()
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

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(Next);
    }
}
