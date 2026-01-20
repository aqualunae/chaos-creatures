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

    [SerializeField]
    private EggListVariable speciesList;

    private void Awake()
    {
        CreatureInstance partyCreature = playerParty.GetIndex(0);
        ChaosCreature speciesRef = speciesList.GetCreature(partyCreature.Species);
        ChaosCreature player = Instantiate(speciesRef, playerLocation.transform.parent);
        player.Initialize(partyCreature);
        player.transform.localScale = playerLocation.transform.localScale;
        player.transform.position = playerLocation.transform.position;
        playerLocation.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
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
