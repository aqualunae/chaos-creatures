using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI victoryField;

    [SerializeField]
    private TextMeshProUGUI lossField;

    [SerializeField]
    private GameObjectVariable progressionRef;

    private void OnEnable()
    {
        ProgressionSystem.ProgressionStats stats = progressionRef.Value.GetComponent<ProgressionSystem>().GetStats();
        victoryField.text = stats.victoryCount.ToString();
        lossField.text = stats.defeatCount.ToString();
    }
}
