using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI titleField;

    [SerializeField]
    private TextMeshProUGUI descriptionField;

    [SerializeField]
    private ColorListVariable colorList;

    [SerializeField]
    private ItemListVariable masterList;

    public void Initialize(string itemTitle)
    {
        if (string.IsNullOrEmpty(itemTitle))
        {
            gameObject.SetActive(false);
            return;
        }

        Item item = masterList.GetItem(itemTitle);

        if (item is not CharmItem)
        {
            Debug.Log("Not a valid charm item.");
            return;
        }

        itemIcon.sprite = item.Sprite;
        itemIcon.color = colorList.GetColor(item.Color);
        titleField.text = item.Title;
        descriptionField.text = item.Description;
    }
}
