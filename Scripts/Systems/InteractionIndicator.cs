using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;

    [SerializeField]
    private float radius = 1;

    [SerializeField]
    private string playerTag;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Sprite desktopIcon;

    [SerializeField]
    private Sprite mobileIcon;

    [SerializeField]
    private bool showIcon = true;

    [SerializeField]
    private IntVariable dprRef;

    private CircleCollider2D interactableField;

    private void Start()
    {
        if (!interactableField)
        {
            interactableField = this.AddComponent<CircleCollider2D>();
        }
        
        interactableField.isTrigger = true;
        interactableField.radius = this.radius;

        icon.sprite = dprRef.Value > 1 ? mobileIcon : desktopIcon;
        icon.color = showIcon ? Color.white : Color.clear;
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
