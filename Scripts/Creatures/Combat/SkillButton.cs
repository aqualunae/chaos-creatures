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

    private Skill skill;
    private SaveableCreature user;
    private SaveableCreature target;

    /// <summary>
    /// Initialize the button with skill data.
    /// </summary>
    public void Initialize(Skill skill, SaveableCreature user, SaveableCreature defaultTarget = null)
    {
        this.skill = skill;
        title.text = skill.Title;
        aspect.text = skill.Aspect.ToString();
        if (skill.Power > 0)
        {
            power.text = skill.Power.ToString();
        }
        else if (skill.Power < 0)
        {
            power.text = $"{skill.Power * -1}";
            power.color = Color.darkGreen; // change this color
        }
        description.text = skill.Description;
        this.user = user;
        this.target = defaultTarget;
    }

    /// <summary>
    /// Change the default target of the skill.
    /// </summary>
    public void SetTarget(SaveableCreature target)
    {
        this.target = target;
    }

    /// <summary>
    /// Activate the skill. Requires a target to be set.
    /// </summary>
    public void UseSkill()
    {
        Debug.Log("clicked");
        Debug.Log(skill.Title);
        if (target == null)
        {
            Debug.Log("No target!");
            return;
        }
        skill.UseSkill(user, target);
    }
}
