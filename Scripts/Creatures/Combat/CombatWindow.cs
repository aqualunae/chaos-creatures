using TMPro;
using UnityEngine;

public class CombatWindow : MonoBehaviour
{
    [SerializeField]
    private ChaosCreature playerLocation;

    [SerializeField]
    private ChaosCreature opponentLocation;

    [SerializeField]
    private CombatStats playerStats;

    [SerializeField]
    private CombatStats opponentStats;

    [SerializeField]
    private TextMeshProUGUI log;

    [SerializeField]
    private GameObject skillsContainer;

    [SerializeField]
    private SkillButton skillButton;

    [SerializeField]
    private Party playerParty;

    private void Awake()
    {
        ChaosCreature partyCreature = playerParty.GetFirst();
        ChaosCreature player = Instantiate(partyCreature, playerLocation.transform.parent);
        player.Name = partyCreature.Name;
        player.SetStats(partyCreature.Stats, partyCreature.Level);
        player.Details = partyCreature.Details;
        player.transform.localScale = playerLocation.transform.localScale;
        player.transform.position = playerLocation.transform.position;
        playerLocation.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        player.Initialize();
        playerStats.Initialize(player.Name, player.Species, player.Level, player.Stats.hp, player.Stats.hp);
        player.FlipFacing();
        log.text = "You've encountered a hostile creature!";

        Skill[] playerSkills = player.GetSkills();
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], player);
        }
    }
}
