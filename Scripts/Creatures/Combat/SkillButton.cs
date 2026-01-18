using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI aspect;

    [SerializeField]
    private TextMeshProUGUI power;

    [SerializeField]
    private TextMeshProUGUI description;

    private Skill skill;
    private ChaosCreature user;
    private ChaosCreature target;

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
            power.color = Color.green;
        }
        description.text = skill.Description;
        user = player;
        target = opponent;
    }

    public void SetTarget(ChaosCreature target)
    {
        this.target = target;
    }

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
