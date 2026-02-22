using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;

public class CreatureEquipmentPanel : MonoBehaviour
{
    [SerializeField]
    private BraceletRenderer braceletRenderer;

    [SerializeField]
    private TextMeshProUGUI braceletTitleField;

    [SerializeField]
    private TextMeshProUGUI braceletColorsField;

    [SerializeField]
    private EquipmentSlot[] slots = new EquipmentSlot[3];

    public void Initialize(Equipment equipment)
    {
        braceletRenderer.Initialize(equipment.braceletStyle, equipment.baseColorTitle, equipment.accentColorTitle);
        braceletTitleField.text = $"{equipment.braceletStyle} Bracelet";
        braceletColorsField.text = $"{equipment.baseColorTitle} / {equipment.accentColorTitle}";

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(equipment.charms[i]);
        }
    }
}
