using System.Collections.Generic;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private StringEvent logUpdateEvent;

    private SaveableCreature player;
    private SaveableCreature opponent;
    private List<SkillButton> skillButtons;

    public void Initialize(SaveableCreature opponent)
    {
        // Initialize the first creature in the player's party.
        player = playerParty.GetByIndex(0);
        CreatureSpecies speciesRef = speciesList.GetSpecies(player.species);
        playerRenderer.Initialize(speciesRef, player.details);
        playerStats.Initialize(player.creatureName, player.species, player.level, player.stats.currentHP, player.stats.hp);
        playerRenderer.FlipFacing();

        // Initialize the opponent
        this.opponent = opponent;
        CreatureSpecies opponentSpecies = speciesList.GetSpecies(opponent.species);
        opponentRenderer.Initialize(opponentSpecies, opponent.details);
        opponentStats.Initialize(opponent.creatureName, opponent.species, opponent.level, opponent.stats.currentHP, opponent.stats.hp);

        log.text = $"You've encountered a hostile {opponent.species}!";

        // Show what skills the player has available.
        skillButtons = new List<SkillButton>();
        Skill[] playerSkills = speciesRef.GetSkills(player.level);
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], player, opponent, this);
            skillButtons.Add(button);
        }

        logUpdateEvent.AddListener(UpdateLog);
    }

    private void OnDisable()
    {
        // since buttons are drawn every time this window is initialized, they need to be removed when the window is closed
        foreach (SkillButton button in skillButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Replace the player creature with an updated version.
    /// </summary>
    public void UpdatePlayer(SaveableCreature update)
    {
        player = update;
        playerStats.UpdateHealth(player.stats.currentHP);
    }

    /// <summary>
    /// Replace the opponent creature with an updated version.
    /// </summary>
    public void UpdateOpponent(SaveableCreature update)
    {
        opponent = update;
        opponentStats.UpdateHealth(opponent.stats.currentHP);
    }

    /// <summary>
    /// Toggle whose turn it is.
    /// </summary>
    /// <param name="state">True for player's turn, false for opponent's turn.</param>
    public void TogglePlayerTurn(bool state)
    {
        foreach (SkillButton button in skillButtons)
        {
            button.GetComponent<Button>().interactable = state;
        }
    }

    /// <summary>
    /// Write a message to the combat window's visible log.
    /// </summary>
    /// <param name="text">Message</param>
    public void UpdateLog(string text)
    {
        log.text = text;
    }

    // TODO
    // - Opponent skill use logic
    // - Update player's creature health when combat ends
    
    // BONUS
    // - Show creature appearance details when you select your own or your opponent's creature.
    // - Inventory interactions
}
