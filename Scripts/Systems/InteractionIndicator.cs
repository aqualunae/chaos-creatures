using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;

    [SerializeField]
    private float radius = 1;

    [SerializeField]
    private string playerTag;

    private CircleCollider2D interactableField;

    private void Start()
    {
        if (!interactableField)
        {
            interactableField = this.AddComponent<CircleCollider2D>();
        }
        
        interactableField.isTrigger = true;
        interactableField.radius = this.radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            popup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        popup.SetActive(false);
    }

    public void SetText(string text)
    {
        TextMeshProUGUI textField = GetComponentInChildren<TextMeshProUGUI>(true);
        textField.text = string.IsNullOrEmpty(text)? "NULL" : text.Replace("_", " ");
    }
}
