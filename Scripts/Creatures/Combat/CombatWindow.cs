using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;

public class CombatWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Position where the player's creature should be rendered.")]
    private CreatureRenderer playerRenderer;

    [SerializeField, Tooltip("Position where the opponent's creature should be rendered.")]
    private CreatureRenderer opponentRenderer;

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

    public void Initialize(SaveableCreature opponent)
    {
        // Initialize the first creature in the player's party.
        SaveableCreature partyCreature = playerParty.GetByIndex(0);
        CreatureSpecies speciesRef = speciesList.GetSpecies(partyCreature.species);
        playerRenderer.Initialize(speciesRef, partyCreature.details);
        playerStats.Initialize(partyCreature.creatureName, partyCreature.species, partyCreature.level, partyCreature.stats.hp, partyCreature.stats.hp);
        playerRenderer.FlipFacing();

        // Initialize the opponent
        CreatureSpecies opponentSpecies = speciesList.GetSpecies(opponent.species);
        opponentRenderer.Initialize(opponentSpecies, opponent.details);
        opponentStats.Initialize(opponent.creatureName, opponent.species, opponent.level, opponent.stats.hp, opponent.stats.hp);

        log.text = "You've encountered a hostile creature!";

        // Show what skills the player has available.
        Skill[] playerSkills = speciesRef.GetSkills(partyCreature.level);
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], partyCreature, opponent);
        }
    }

    private void OnDisable()
    {
        // since buttons are drawn every time this window is initialized, they need to be removed when the window is closed
        SkillButton[] buttons = skillsContainer.GetComponentsInChildren<SkillButton>();
        foreach (SkillButton button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // TODO
    // - Skill use logic
    // - Update player's creature health when combat ends
    
    // BONUS
    // - Show creature appearance details when you select your own or your opponent's creature.
    // - Inventory interactions
}
