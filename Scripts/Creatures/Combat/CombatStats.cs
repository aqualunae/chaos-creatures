using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
        healthField.text = $"{(int)currentHealth}/{maxHealth}";
    }
}
