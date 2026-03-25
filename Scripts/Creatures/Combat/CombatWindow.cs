using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Linq;

public class CombatWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Position where the player's creature should be rendered.")]
    private CreatureRenderer playerRenderer;

    [SerializeField, Tooltip("Position where the opponent's creature should be rendered.")]
    private CreatureRenderer opponentRenderer;

    [SerializeField, Tooltip("Window that will display the combat stats of the player's creature.")]
    private CombatStats playerStats;

    [SerializeField, Tooltip("Window that will display the combat stats of the opponent's creature.")]
    private CombatStats opponentStats;

    [SerializeField, Tooltip("Text field for displaying in-combat messages.")]
    private TextMeshProUGUI log;

    [SerializeField, Tooltip("Container to hold skill buttons.")]
    private GameObject skillsContainer;

    [SerializeField, Tooltip("Prefab skill button to instantiate.")]
    private SkillButton skillButton;

    [SerializeField, Tooltip("Window that will display the visual details of the player's creature.")]
    private CreatureDetailsWindow playerDetails;

    [SerializeField, Tooltip("Window that will display the visual details of the opponent's creature.")]
    private CreatureDetailsWindow opponentDetails;

    [SerializeField, Tooltip("Window that holds the player's root actions, such as Skills, Items, and Party.")]
    private GameObject actionsMenu;

    [SerializeField, Tooltip("Window where the skill buttons are rendered.")]
    private GameObject skillsMenu;

    [SerializeField, Tooltip("Reference to the player.")]
    private GameObjectVariable playerRef;

    [SerializeField, Tooltip("List of possible species, used to instantiate creatures to render.")]
    private SpeciesListVariable speciesList;

    [SerializeField, Tooltip("Event called when something happens in combat and needs to be displayed to the player.")]
    private StringEvent logUpdateEvent;

    [SerializeField, Tooltip("Event used to change the state of the game.")]
    private GameStateEvent pauzeEvent;

    [SerializeField, Tooltip("Sprite swapper that affects skills targeting the player.")]
    private SpriteSwapper playerSkillSwapper;

    [SerializeField, Tooltip("Sprite swapper that affects skills targeting the opponent.")]
    private SpriteSwapper opponentSkillSwapper;

    [SerializeField, Tooltip("Reference to the audio source.")]
    private GameObjectVariable audioRef;

    [SerializeField, Tooltip("Event that is fired when the player makes progress in the game.")]
    private StringEvent progressionTrigger;

    [SerializeField]
    private SkillDetails skillDetails;

    private SaveableCreature player;
    private SaveableCreature opponent;
    private List<SkillButton> skillButtons;
    private CreatureSpecies opponentSpecies;
    private CreatureSpecies playerSpecies;
    private Vector3 actionMenuLocation;
    private Button[] actionButtons;
    private Button endCombatButton;
    private AudioSource audioSource;
    private Party opponentParty;
    private string opponentName;
    private Skill[] playerSkills;
    
    /// <summary>
    /// Get the creature that is currently in combat for the player.
    /// </summary>
    public SaveableCreature GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// Get the creature that the player is in combat against.
    /// </summary>
    public SaveableCreature GetOpponent()
    {
        return opponent;
    }

    /// <summary>
    /// Attempt to befriend the current opponent.
    /// </summary>
    /// <returns>True if successful</returns>
    public bool BefriendCreature()
    {
        return playerRef.Value.GetComponent<Party>().AddToParty(opponent);
    }

    public bool StoreCreature()
    {
        return playerRef.Value.GetComponent<Party>().AddToStorage(opponent);
    }

    /// <summary>
    /// Gets the player's current creature for combat. Renders the creature's visuals, stats, and skills.
    /// </summary>
    private void RefreshPlayer()
    {
        // select the first party creature that is able to fight.
        Party playerParty = playerRef.Value.GetComponent<Party>();
        for (int i = 0; i < playerParty.Creatures.Count; i++)
        {
            player = playerParty.GetByIndex(i);
            if (player != null && player.stats.currentHP > 0 && player.level > 0)
            {
                break;
            }
        }
        
        // initialize all combat components
        playerSpecies = speciesList.GetSpecies(player.species);
        playerRenderer.Initialize(playerSpecies, player.details);
        playerDetails.Initialize(player);
        playerStats.Initialize(player);
        playerRenderer.FlipFacing();

        // remove skill buttons from a previous creature or combat instance
        DisableSkillButtons();

        // Show what skills the player has available.
        skillButtons = new List<SkillButton>();
        playerSkills = playerSpecies.GetSkills(player.level);
        for (int i = 0; i < playerSkills.Length; i++)
        {
            SkillButton button = Instantiate(skillButton, skillsContainer.transform);
            button.Initialize(playerSkills[i], i);
            button.GetComponent<Button>().interactable = true;
            skillButtons.Add(button);
        }

        SelectFirstSkill();
    }

    /// <summary>
    /// Prepares the combat window.
    /// </summary>
    /// <param name="opponent">Creature that the player is in combat against.</param>
    public void Initialize(SaveableCreature opponent)
    {
        this.opponent = opponent;

        UpdateLog($"You've encountered a hostile { opponent.species }!");

        Initialize();
    }

    public void Initialize(Party opponentParty)
    {
        this.opponent = opponentParty.GetByIndex(0);
        this.opponentParty = opponentParty;
        opponentName = opponentParty.GetComponent<Dialogue>().CharacterName;

        UpdateLog($"{ opponentName } sent out { opponent.creatureName }!");

        Initialize();
    }

    private void Initialize()
    {
        // Initialize the opponent
        opponentSpecies = speciesList.GetSpecies(opponent.species);
        opponentRenderer.Initialize(opponentSpecies, opponent.details);
        opponentDetails.Initialize(opponent);
        opponentStats.Initialize(opponent);
        opponentRenderer.gameObject.SetActive(true);

        RefreshPlayer();

        // set the actions menu position that it should be returned to when the visual details view is closed
        actionMenuLocation = actionsMenu.transform.localPosition;

        // get the buttons in the actions menu and make sure they're interactable
        actionButtons = actionsMenu.GetComponentsInChildren<Button>();
        endCombatButton = actionButtons[^1];
        endCombatButton.GetComponentInChildren<TextMeshProUGUI>().text = "Flee";
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = true;
        }

        // set up logging and pauze the overworld
        logUpdateEvent.AddListener(UpdateLog);
        pauzeEvent.Invoke(GameState.CombatWindow);

        // set up the audio source and play the opponent's call
        audioSource = audioRef.Value.GetComponent<AudioSource>();
        audioSource.PlayOneShot(opponentSpecies.Call);

        // todo: speed contest to determine if the player or opponent has the first turn
    }

    /// <summary>
    /// Disable all skill buttons, for example when combat has ended.
    /// </summary>
    private void DisableSkillButtons()
    {
        if (skillButtons != null)
        {
            foreach (SkillButton button in skillButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Clean up listeners and unpauze the game when combat ends.
    /// </summary>
    private void OnDisable()
    {
        logUpdateEvent.RemoveListener(UpdateLog);
        endCombatButton.onClick.RemoveListener(HealOpponent);
        pauzeEvent.Invoke(GameState.Overworld);
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
            PlayerDefeat();
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
            PlayerVictory();
        }
    }

    /// <summary>
    /// Toggle whose turn it is.
    /// </summary>
    /// <param name="state">True for player's turn, false for opponent's turn.</param>
    public void TogglePlayerTurn(bool state)
    {
        // skills should only be interactable during the player's turn
        foreach (SkillButton button in skillButtons)
        {
            button.GetComponent<Button>().interactable = state;
        }

        // if it is the player's turn, select the first skill they have available
        if (state)
        {
            SelectFirstSkill();
        }

        // if it's the opponent's turn and the opponent is able to fight, start their turn
        if (!state && opponent.stats.currentHP > 0)
        {
            skillDetails.Initialize(null);
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
        // pause so that the player can process
        yield return new WaitForSeconds(1);

        // get appropriate skills and choose one at random
        Skill[] skills = opponentSpecies.GetSkills(opponent.level);
        int index = UnityEngine.Random.Range(0, skills.Length);

        if (skills[index].Sound != null)
        {
            audioSource.PlayOneShot(skills[index].Sound);
        }

        // the UpdatePlayer and UpdateOpponent methods will end combat if a creature's health is reduced to 0
        if (!skills[index].TargetSelf)
        {
            // if the skill is not self-targeting, use the skill on the player and update the player with the result
            playerSkillSwapper.sprites = skills[index].Sprites;
            playerSkillSwapper.GetComponent<Animator>().Play("Skill");
            SaveableCreature opponentTarget = skills[index].UseSkill(opponent, player, logUpdateEvent);
            UpdatePlayer(opponentTarget);
        }
        else
        {
            // if the skill is self-targeting, use it on the opponent and update them
            opponentSkillSwapper.sprites = skills[index].Sprites;
            opponentSkillSwapper.GetComponent<Animator>().Play("Skill");
            SaveableCreature opponentTarget = skills[index].UseSkill(opponent, opponent, logUpdateEvent);
            UpdateOpponent(opponentTarget);
        }

        // if the player has not been defeated, it's their turn again
        if (player.stats.currentHP > 0)
        {
            TogglePlayerTurn(true);
        }

        yield return null;
    }

    /// <summary>
    /// The player selects a skill and uses it. Called by Skill Button primarily.
    /// </summary>
    /// <param name="index">Index of skill to use</param>
    public void PlayerSkill(int index)
    {
        Skill skill = playerSkills[index];

        SaveableCreature skillTarget = skill.TargetSelf ? player : opponent;

        // Apply skill effects
        SaveableCreature updatedTarget = skill.UseSkill(player, skillTarget, logUpdateEvent);
        if (skill.Sound != null)
        {
            audioSource.PlayOneShot(skill.Sound);
        }

        if (!skill.TargetSelf)
        {
            opponentSkillSwapper.sprites = skill.Sprites;
            opponentSkillSwapper.GetComponent<Animator>().Play("Skill");
            UpdateOpponent(updatedTarget);
        }
        else
        {
            playerSkillSwapper.sprites = skill.Sprites;
            playerSkillSwapper.GetComponent<Animator>().Play("Skill");
            UpdatePlayer(updatedTarget);
        }

        // The player's turn is over now that they've used a skill.
        TogglePlayerTurn(false);
    }

    /// <summary>
    /// Display a victory message and calculate experience.
    /// </summary>
    private void PlayerVictory()
    {
        // log that the opponent has been defeated
        string victoryLog = $"You have defeated the {opponent.species}! ";

        // calculate experience
        int expEarned = (int)Math.Pow(opponent.level * 3, 3);
        player.stats.exp += expEarned;
        playerStats.UpdateExperience(player.stats.exp);
        victoryLog += $"{expEarned} exp earned. ";
        int levelThreshhold = CreatureUtility.GetExperienceThreshold(player.level);
        if (player.stats.exp >= levelThreshhold)
        {
            int leftoverExp = player.stats.exp - levelThreshhold;
            player.stats = playerSpecies.IncrementStats(player.stats);
            player.stats.exp = leftoverExp;
            player.level++;
            victoryLog += "Level up!";

            progressionTrigger.Invoke($"Level: {player.level}");
        }

        // hide the opponent renderer to show they're no longer able to fight
        opponentRenderer.gameObject.SetActive(false);

        // check if the opponent has a next creature available
        if (opponentParty != null)
        {
            SaveableCreature[] healthyCreatures = opponentParty.Creatures.Values.Where(creature => creature?.stats.currentHP > 0).ToArray();
            if (healthyCreatures.Length > 0)
            {
                opponent = healthyCreatures[0];
                UpdateLog($"{ opponentName } sent out { opponent.creatureName }!");
                Initialize();
            }
            else
            {
                progressionTrigger.Invoke($"Victory: { opponentName }");

                UpdateLog(victoryLog);
                EndCombat(true);
            }
        }
        else
        {
            progressionTrigger.Invoke("Victory: Random Encounter");

            UpdateLog(victoryLog);
            EndCombat(true);
        }
        
    }

    /// <summary>
    /// Disable all buttons and then turn the Flee button into an End button.
    /// </summary>
    /// <param name="victory"></param>
    private void EndCombat(bool victory)
    {
        // switches the menu to skills, which prevents us from having to disable party and item buttons
        SelectFirstSkill();

        // disable action buttons
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].interactable = false;
        }

        // disable skill buttons
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].GetComponent<Button>().interactable = false;
        }
        skillDetails.Initialize(null);

        // update and select the button that ends combat
        endCombatButton.interactable = true;
        endCombatButton.GetComponentInChildren<TextMeshProUGUI>().text = "End";
        endCombatButton.Select();
        endCombatButton.onClick.AddListener(HealOpponent);
        if (!victory)
        {
            endCombatButton.onClick.AddListener(ConfirmDefeat);
        }
    }

    private void HealOpponent()
    {
        if (opponentParty != null)
        {
            opponentParty.HealAll();
        }
    }

    /// <summary>
    /// Display a defeat log.
    /// </summary>
    private void PlayerDefeat()
    {
        // disable skills
        foreach (SkillButton button in skillButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }

        // prepare to log that your creature has been defeated
        string defeatLog = $"{ player.creatureName } is no longer able to fight!";
        
        // get the number of creatures the player has that are able to enter combat
        Party playerParty = playerRef.Value.GetComponent<Party>();
        SaveableCreature[] healthyCreatures = playerParty.Creatures.Values.Where(creature => creature?.stats.currentHP > 0 && creature?.level > 0).ToArray();
        if (healthyCreatures.Length > 0)
        {
            // if there are healthy creatures remaining, open the party tab
            TabSwitcher switcher = actionsMenu.GetComponent<TabSwitcher>();
            switcher.AutoSwitch(2);
        }
        else
        {
            progressionTrigger.Invoke("Defeat");

            // if there are no healthy creatures, the player is defeated
            defeatLog += " You don't have any more creatures that are able to fight.";
            EndCombat(false);
        }

        // tell the player what has happened
        UpdateLog(defeatLog);
    }

    /// <summary>
    /// Return the player to a safe position.
    /// </summary>
    private void ConfirmDefeat()
    {
        // warp the player back to the tent and heal their creatures
        playerRef.Value.GetComponent<WarpSelf>().WarpToTarget();
        playerRef.Value.GetComponent<Party>().HealAll();

        endCombatButton.onClick.RemoveListener(ConfirmDefeat);

        // close the combat window
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Obsolete: switching tabs is now handled by TabSwitcher
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

    /// <summary>
    /// Selects first skill button, if available. Otherwise, selects an action button.
    /// </summary>
    public void SelectFirstSkill()
    {
        // open the skills menu
        TabSwitcher switcher = actionsMenu.GetComponent<TabSwitcher>();
        switcher.AutoSwitch(0);
        
        // if skill buttons have been rendered, select the first one
        if (skillButtons != null && skillButtons.Count > 0)
        {
            skillButtons[0].GetComponent<Button>().Select();
        }
        else
        {
            // otherwise, select an available action button
            Button[] actionButtons = actionsMenu.GetComponentsInChildren<Button>();
            if (actionButtons[0].interactable)
            {
                actionButtons[0].Select();
            }
            else if (actionButtons[1].interactable)
            {
                actionButtons[1].Select();
            }
            else
            {
                actionButtons[^1].Select();
            }
        }
    }

    /// <summary>
    /// Use an item during combat.
    /// </summary>
    /// <param name="itemData">Item to be used</param>
    /// <returns>True if the item was successfully used and needs to be reduced in inventory</returns>
    public bool UseItem(Item itemData)
    {
        bool itemWasUsed = false;

        // determine which creature should be targeted by the item
        SaveableCreature target = GetOpponent();
        bool targetSelf = false;
        if (itemData is CombatItem)
        {
            CombatItem combatItem = itemData as CombatItem;
            if (combatItem.TargetSelf)
            {
                target = GetPlayer();
                targetSelf = true;
            }
        }

        // attempt to use the item
        Item.UseItemResult result = itemData.UseItem(target);
        string log = result.log;

        // if using a bracelet was successful, handle friendship
        if (itemData is Bracelet && result.success)
        {
            if (BefriendCreature())
            {
                log += " It was added to your party.";
                itemWasUsed = true;
                EndCombat(true);
            }
            else if (StoreCreature())
            {
                log += " It was added to your storage.";
                itemWasUsed = true;
                EndCombat(true);
            }
            else
            {
                log += " But there's no room in your storage!";
                TogglePlayerTurn(false);
            }
        }
        // if using a bracelet was unsuccessful, the player's turn is over
        else if (itemData is Bracelet && !result.success)
        {
            itemWasUsed = true;
            TogglePlayerTurn(false);
        }
        // if a combat item was used successfully, update the target and end the player's turn
        else if (result.success)
        {
            if (targetSelf)
            {
                UpdatePlayer(result.target);
            }
            else
            {
                UpdateOpponent(result.target);
            }
            TogglePlayerTurn(false);
            itemWasUsed = true;
        }
        // if an attempt was made to use a combat item and it was unsuccessful
        // that's probably a code error, so don't reduce the item stack or end the player's turn

        // log the result and return whether the inventory stack should be reduced
        UpdateLog(log);
        return itemWasUsed;
    }

    /// <summary>
    /// Call this method when the order of the player's party changes
    /// </summary>
    public void SwitchCreature()
    {
        // update the player's active creature
        RefreshPlayer();

        // tell the player what's going on
        UpdateLog($"You sent out {player.creatureName}.");

        // switching creatures consumes a turn; it is now the opponent's turn
        // when the player is switching creatures as a result of being defeated, a speed contest should occur
        TogglePlayerTurn(false);
    }

    /// <summary>
    /// When a skill is selected, show its details in the Skill Details panel.
    /// </summary>
    /// <param name="index">Skill index.</param>
    public void SelectSkill(int index)
    {
        if (playerSkills.Length > index)
        {
            skillDetails.Initialize(playerSkills[index]);
        }
        else
        {
            skillDetails.Initialize(null);
        }
    }
}
