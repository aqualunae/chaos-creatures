using System.Collections;
using Assets.Scripts.Creatures.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to display data about one creature during combat.
/// </summary>
public class CombatStats : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameField;

    [SerializeField]
    private TextMeshProUGUI speciesField;

    [SerializeField]
    private TextMeshProUGUI levelField;

    [SerializeField]
    private TextMeshProUGUI healthField;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private TextMeshProUGUI experienceField;

    [SerializeField]
    private Slider experienceSlider;

    [SerializeField, Range(1, 20)]
    private float sliderSpeed = 10;

    private int maxHealth;
    private int maxExperience;

    private float previousHealth;
    private float currentHealth;

    private float previousExperience;
    private float currentExperience;
    private int level;

    /// <summary>
    /// Pass in creature data so it can be rendered in the Combat Stats panel.
    /// </summary>
    public void Initialize(string name, string species, int level, Stats stats)
    {
        nameField.text = name;
        speciesField.text = species;
        levelField.text = $"Lvl {level}";

        maxHealth = (int)stats.hp;
        previousHealth = stats.currentHP;
        currentHealth = stats.currentHP;
        healthField.text = $"{(int)stats.currentHP}/{(int)maxHealth} HP";
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = stats.currentHP;

        maxExperience = CreatureUtility.GetExperienceThreshold(level);
        previousExperience = stats.exp;
        currentExperience = stats.exp;
        experienceField.text = $"{stats.exp}/{maxExperience} EXP";
        experienceSlider.minValue = 0;
        experienceSlider.maxValue = maxExperience;
        experienceSlider.value = stats.exp;
        this.level = level;
    }

    /// <summary>
    /// Update creature's health, such as when attacked.
    /// </summary>
    public void UpdateHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
        healthField.text = $"{(int)currentHealth}/{maxHealth}";
    }

    public void UpdateExperience(int currentExperience)
    {
        this.currentExperience = currentExperience;
        experienceField.text = $"{(int)currentExperience}/{maxExperience}";
    }

    private void Update()
    {
        if (currentHealth != previousHealth)
        {
            float transitionHealth = Mathf.MoveTowards(previousHealth, currentHealth, sliderSpeed * Time.deltaTime);
            healthSlider.value = transitionHealth;
            previousHealth = transitionHealth;
        }

        if (currentExperience != previousExperience)
        {
            float transitionExperience = Mathf.MoveTowards(previousExperience, currentExperience, 5 * sliderSpeed * level * Time.deltaTime);
            experienceSlider.value = transitionExperience;
            previousExperience = transitionExperience;
        }
    }
}
