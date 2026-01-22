using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;

public class CombatWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Position where the player's creature should be rendered.")]
    private GameObject playerLocation;

    [SerializeField, Tooltip("Position where the opponent's creature should be rendered.")]
    private GameObject opponentLocation;

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
    private SpeciesListVariable speciesList;

    [SerializeField, Tooltip("")]
    private CreatureRenderer rendererPrefab;

    private void Awake()
    {
        // Initialize and draw the first creature in the player's party.
        SaveableCreature partyCreature = playerParty.GetByIndex(0);
        CreatureSpecies speciesRef = speciesList.GetSpecies(partyCreature.species);
        CreatureRenderer playerRenderer = Instantiate(rendererPrefab, playerLocation.transform.parent);
        playerRenderer.Initialize(speciesRef, partyCreature.details);
        playerRenderer.transform.localScale = playerLocation.transform.localScale;
        playerRenderer.transform.position = playerLocation.transform.position;
        playerLocation.SetActive(false);
        playerRenderer.gameObject.SetActive(true);
        playerStats.Initialize(partyCreature.creatureName, partyCreature.species, partyCreature.level, partyCreature.stats.hp, partyCreature.stats.hp);
        playerRenderer.FlipFacing();
        log.text = "You've encountered a hostile creature!";

        // Show what skills the player has available.
        Skill[] playerSkills = speciesRef.GetSkills(partyCreature.level);
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], partyCreature);
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
