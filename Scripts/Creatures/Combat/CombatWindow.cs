using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    [SerializeField]
    private CreatureDetailsWindow playerDetails;

    [SerializeField]
    private CreatureDetailsWindow opponentDetails;

    [SerializeField]
    private GameObject actionsMenu;

    [SerializeField]
    private GameObject skillsMenu;

    [SerializeField, Tooltip("Reference to the player's party.")]
    private Party playerParty;

    [SerializeField, Tooltip("List of possible species, used to instantiate creatures to render.")]
    private SpeciesListVariable speciesList;

    [SerializeField]
    private StringEvent logUpdateEvent;

    private SaveableCreature player;
    private SaveableCreature opponent;
    private List<SkillButton> skillButtons;
    private CreatureSpecies opponentSpecies;
    private CreatureSpecies playerSpecies;
    private Vector3 actionMenuLocation;

    public void Initialize(SaveableCreature opponent)
    {
        // Initialize the first creature in the player's party.
        player = playerParty.GetByIndex(0);
        playerSpecies = speciesList.GetSpecies(player.species);
        playerRenderer.Initialize(playerSpecies, player.details);
        playerDetails.Initialize(player);
        playerStats.Initialize(player.creatureName, player.species, player.level, player.stats.currentHP, player.stats.hp);
        playerRenderer.FlipFacing();

        // Initialize the opponent
        this.opponent = opponent;
        opponentSpecies = speciesList.GetSpecies(opponent.species);
        opponentRenderer.Initialize(opponentSpecies, opponent.details);
        opponentDetails.Initialize(opponent);
        opponentStats.Initialize(opponent.creatureName, opponent.species, opponent.level, opponent.stats.currentHP, opponent.stats.hp);
        opponentRenderer.gameObject.SetActive(true);

        log.text = $"You've encountered a hostile {opponent.species}!";

        // Show what skills the player has available.
        skillButtons = new List<SkillButton>();
        Skill[] playerSkills = playerSpecies.GetSkills(player.level);
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], player, opponent, this);
            skillButtons.Add(button);
        }

        // set the actions menu position that it should be returned to when the visual details view is closed
        actionMenuLocation = actionsMenu.transform.localPosition;
        logUpdateEvent.AddListener(UpdateLog);
    }

    private void OnDisable()
    {
        // since buttons are drawn every time this window is initialized, they need to be removed when the window is closed
        foreach (SkillButton button in skillButtons)
        {
            button.gameObject.SetActive(false);
        }
        ToggleDetails(false);
    }

    /// <summary>
    /// Replace the player creature with an updated version.
    /// </summary>
    public void UpdatePlayer(SaveableCreature update)
    {
        player = update;
        playerStats.UpdateHealth(player.stats.currentHP);
        if (player.stats.currentHP == 0)
        {
            StartCoroutine(PlayerDefeat());
        }
    }

    /// <summary>
    /// Replace the opponent creature with an updated version.
    /// </summary>
    public void UpdateOpponent(SaveableCreature update)
    {
        opponent = update;
        opponentStats.UpdateHealth(opponent.stats.currentHP);
        if (opponent.stats.currentHP == 0)
        {
            StartCoroutine(PlayerVictory());
        }
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

        if (!state && opponent.stats.currentHP > 0)
        {
            StartCoroutine(OpponentSkill());
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

    /// <summary>
    /// The opponent randomly selects a skill and uses it.
    /// </summary>
    private IEnumerator OpponentSkill()
    {
        yield return new WaitForSeconds(1);

        Skill[] skills = opponentSpecies.GetSkills(opponent.level);
        int index = UnityEngine.Random.Range(0, skills.Length);

        if (!skills[index].TargetSelf)
        {
            SaveableCreature opponentTarget = skills[index].UseSkill(opponent, player, logUpdateEvent);
            UpdatePlayer(opponentTarget);
        }
        else
        {
            SaveableCreature opponentTarget = skills[index].UseSkill(opponent, opponent, logUpdateEvent);
            UpdateOpponent(opponentTarget);
        }

        TogglePlayerTurn(true);

        yield return null;
    }

    /// <summary>
    /// Display a victory message and calculate experience.
    /// </summary>
    private IEnumerator PlayerVictory()
    {
        // log that the opponent has been defeated
        string victoryLog = $"You have defeated the {opponent.species}! ";

        // calculate experience
        int expEarned = (int)Math.Pow(opponent.level * 3, 3);
        player.stats.exp += expEarned;
        victoryLog += $"{expEarned} exp earned. ";
        int levelThreshhold = (int)Math.Pow(player.level * 5, 3) - (int)Math.Pow((player.level - 1) * 5, 3);
        if (player.stats.exp >= levelThreshhold)
        {
            int leftoverExp = player.stats.exp - levelThreshhold;
            player.stats = playerSpecies.IncrementStats(player.stats);
            player.stats.exp = leftoverExp;
            player.level++;
            victoryLog += "Level up!";
        }

        // hide the opponent renderer to show they're no longer able to fight
        opponentRenderer.gameObject.SetActive(false);

        // wait for the player to read the log
        UpdateLog(victoryLog);
        yield return new WaitForSeconds(3);

        // close the combat window
        gameObject.SetActive(false);
        yield return null;
    }

    /// <summary>
    /// Display a defeat log and return the player to a safe position.
    /// </summary>
    private IEnumerator PlayerDefeat()
    {
        // log that your creature has been defeated
        string defeatLog = $"{player.creatureName} is no longer able to fight!";
        UpdateLog(defeatLog);
        yield return new WaitForSeconds(3);

        // ask the player if theyd like to use their next creature, if applicable

        // otherwise, warp the player back to the tent and heal their creatures
        playerParty.GetComponent<Warper>().WarpToTarget();
        playerParty.HealAll();

        // close the combat window
        gameObject.SetActive(false);
        yield return null;
    }

    /// <summary>
    /// Toggle display of the creatures' visual details.
    /// </summary>
    /// <param name="state">True to display details, false to go back to displaying skills.</param>
    public void ToggleDetails(bool state)
    {
        if (state)
        {
            actionsMenu.transform.localPosition = new Vector3(0, actionMenuLocation.y);
        }
        else
        {
            actionsMenu.transform.localPosition = actionMenuLocation;
        }

        skillsMenu.SetActive(!state);
        playerDetails.gameObject.SetActive(state);
        opponentDetails.gameObject.SetActive(state);
    }
    
    // BONUS
    // - Show creature appearance details when you select your own or your opponent's creature.
    // - Inventory interactions
}
