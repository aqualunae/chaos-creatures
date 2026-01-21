using TMPro;
using UnityEngine;

public class CombatWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Position where the player's creature should be rendered.")]
    private ChaosCreature playerLocation;

    [SerializeField, Tooltip("Position where the opponent's creature should be rendered.")]
    private ChaosCreature opponentLocation;

    [SerializeField, Tooltip("Window that will display the stats of the player's creature.")]
    private CombatStats playerStats;

    [SerializeField, Tooltip("Window that will display the stats of the opponent's creature.")]
    private CombatStats opponentStats;

    [SerializeField, Tooltip("Text field for displaying in-combat messages.")]
    private TextMeshProUGUI log;

    [SerializeField, Tooltip("Container to hold skill buttons.")]
    private GameObject skillsContainer;

    [SerializeField, Tooltip("Prefab skill button to instantiate.")]
    private SkillButton skillButton;

    [SerializeField, Tooltip("Reference to the player's party.")]
    private Party playerParty;

    [SerializeField, Tooltip("List of possible species, used to instantiate creatures to render.")]
    private EggListVariable speciesList;

    private void Awake()
    {
        // Initialize and draw the first creature in the player's party.
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

        // Show what skills the player has available.
        Skill[] playerSkills = player.GetSkills();
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], player);
        }
    }

    // TODO
    // - Initialize opponent creature
    // - Skill use logic
    // - Update player's creature health when combat ends
    
    // BONUS
    // - Show creature appearance details when you select your own or your opponent's creature.
    // - Inventory interactions
}
