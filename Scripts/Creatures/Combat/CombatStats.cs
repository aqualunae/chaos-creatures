using System.Collections;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to display data about one creature during combat.
/// </summary>
public class CombatStats : MonoBehaviour
{
    [SerializeField, Tooltip("The field for the given name of the creature.")]
    private TextMeshProUGUI nameField;

    [SerializeField, Tooltip("The field for the species name of the creature.")]
    private TextMeshProUGUI speciesField;

    [SerializeField, Tooltip("The field for the level of the creature.")]
    private TextMeshProUGUI levelField;

    [SerializeField, Tooltip("The field for the health (HP) of the creature.")]
    private TextMeshProUGUI healthField;

    [SerializeField, Tooltip("A slider that displays the health (HP) of the creature.")]
    private Slider healthSlider;

    [SerializeField, Tooltip("The field for the experience points of the creature.")]
    private TextMeshProUGUI experienceField;

    [SerializeField, Tooltip("A slider that displays the experience the creature has relative to the amount they need to reach the next level.")]
    private Slider experienceSlider;

    [SerializeField, Range(1, 20), Tooltip("When health or experience values change, how fast should the sliders move? Higher numbers mean faster speeds.")]
    private float sliderSpeed = 10;

    // maximum values for health and experience
    private int maxHealth;
    private int maxExperience;

    // actual current values of health and experience
    private float currentHealth;
    private float currentExperience;

    // previous values of health and experience, used for tweening the sliders
    private float previousHealth;
    private float previousExperience;

    // level, used for calculations
    private int level;

    /// <summary>
    /// Pass in creature data so it can be rendered in the Combat Stats panel.
    /// </summary>
    public void Initialize(SaveableCreature creature)
    {
        // name, species, and level
        nameField.text = name;
        speciesField.text = creature.species;
        this.level = creature.level;
        levelField.text = $"Lvl {level}";

        if (level > 0)
        {
            Stats stats = creature.stats;
            // values that will be used to calculate health
            maxHealth = (int)stats.hp;
            previousHealth = stats.currentHP;
            currentHealth = stats.currentHP;

            // health rendering
            healthField.text = $"{(int)stats.currentHP}/{(int)maxHealth} HP";
            healthSlider.gameObject.SetActive(true);
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = stats.currentHP;

            // values that will be used to calculate experience on victory
            maxExperience = CreatureUtility.GetExperienceThreshold(level);
            previousExperience = stats.exp;
            currentExperience = stats.exp;
            
            // experience rendering
            experienceField.gameObject.SetActive(true);
            experienceField.text = $"{stats.exp}/{maxExperience} EXP";
            experienceSlider.gameObject.SetActive(true);
            experienceSlider.minValue = 0;
            experienceSlider.maxValue = maxExperience;
            experienceSlider.value = stats.exp;
        }
        else
        {
            healthField.text = $"{ creature.details.eggSteps }/{ CreatureUtility.EggSteps } steps";
            healthSlider.gameObject.SetActive(false);
            experienceField.gameObject.SetActive(false);
            experienceSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Update creature's health, such as when attacked.
    /// </summary>
    public void UpdateHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
        healthField.text = $"{(int)currentHealth}/{maxHealth}";
    }

    /// <summary>
    /// Update creature's experience, such as on victory.
    /// </summary>
    public void UpdateExperience(int currentExperience)
    {
        this.currentExperience = currentExperience;
        experienceField.text = $"{(int)currentExperience}/{maxExperience}";
    }

    private void Update()
    {
        // if the previous health value hasn't caught up to the current value
        // gradually move it in that direction
        if (currentHealth != previousHealth)
        {
            float transitionHealth = Mathf.MoveTowards(previousHealth, currentHealth, sliderSpeed * Time.deltaTime);
            healthSlider.value = transitionHealth;
            previousHealth = transitionHealth;
        }

        // if the previous experience value hasn't caught up to the current value
        // gradually move it in that direction
        if (currentExperience != previousExperience)
        {
            float transitionExperience = Mathf.MoveTowards(previousExperience, currentExperience, 5 * sliderSpeed * level * Time.deltaTime);
            experienceSlider.value = transitionExperience;
            previousExperience = transitionExperience;
        }
    }
}
