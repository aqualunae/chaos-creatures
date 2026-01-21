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

    private int maxHealth;

    /// <summary>
    /// Pass in creature data so it can be rendered in the Combat Stats panel.
    /// </summary>
    public void Initialize(string name, string species, int level, float currentHealth, float maxHealth)
    {
        nameField.text = name;
        speciesField.text = species;
        levelField.text = $"Lvl {level}";
        healthField.text = $"{(int)currentHealth}/{(int)maxHealth}";
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        this.maxHealth = (int)maxHealth;
    }

    /// <summary>
    /// Update creature's health, such as when attacked.
    /// </summary>
    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
        healthField.text = $"{(int)currentHealth}/{maxHealth}";
    }
}
