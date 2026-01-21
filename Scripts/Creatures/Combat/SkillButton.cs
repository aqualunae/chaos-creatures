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
    private ChaosCreature user;
    private ChaosCreature target;

    /// <summary>
    /// Initialize the button with skill data.
    /// </summary>
    /// <param name="skill">Skill to display</param>
    /// <param name="player">Skill user</param>
    /// <param name="opponent">Default skill target</param>
    public void Initialize(Skill skill, ChaosCreature player, ChaosCreature opponent = null)
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
        user = player;
        target = opponent;
    }

    /// <summary>
    /// Change the default target of the skill.
    /// </summary>
    public void SetTarget(ChaosCreature target)
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
