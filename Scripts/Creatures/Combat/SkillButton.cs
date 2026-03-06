using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    [SerializeField, Tooltip("Field that displays the title.")]
    private TextMeshProUGUI title;

    [SerializeField, Tooltip("Field that displays the aspect.")]
    private TextMeshProUGUI aspect;

    [SerializeField, Tooltip("Field that displays the power.")]
    private TextMeshProUGUI power;

    [SerializeField, Tooltip("Field that displays the description, where possible.")]
    private TextMeshProUGUI description;

    [SerializeField, Tooltip("Color used for the power of the skill if it heals.")]
    private Color healColor = Color.darkGreen;

    [SerializeField, Tooltip("Event that fires when something happens in combat.")]
    private StringEvent logUpdateEvent;

    // variables that allow the skill to be used
    private int index;
    private SelectionListener skillListener;

    /// <summary>
    /// Initialize the button with skill data.
    /// </summary>
    public void Initialize(Skill skill, int index)
    {
        this.index = index;
        skillListener = GetComponentInParent<SelectionListener>();

        title.text = skill.Title;
        aspect.text = skill.Aspect.ToString();
        if (skill.Power > 0)
        {
            power.text = skill.Power.ToString();
        }
        else if (skill.Power < 0)
        {
            // if the skill heals, its power will be negative
            // instead of displaying it as a negative number, we display it as a positive number in a different color, such as green
            power.text = $"{skill.Power * -1}";
            power.color = healColor;
        }
        description.text = skill.Description;
    }

    /// <summary>
    /// Activate the skill.
    /// </summary>
    public void UseSkill()
    {
        skillListener.OnActivate(index);
    }

    public void SelectSkill()
    {
        skillListener.OnSelect(index);
    }
}
