using TMPro;
using UnityEngine;

public class SkillDetails : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleField;

    [SerializeField]
    private TextMeshProUGUI infoField;

    public void Initialize(Skill skill)
    {
        // if the skill is null, don't show anything
        titleField.gameObject.SetActive(skill != null);
        infoField.gameObject.SetActive(skill != null);
        if (skill == null)
        {
            return;
        }

        // otherwise, format and display details
        titleField.text = skill.Title;
        infoField.text = $"{skill.Description}";
    }
}
