using UnityEngine;

[CreateAssetMenu(fileName = "Bracelet_", menuName = "Items/Bracelet")]
public class Bracelet : Item
{
    public enum BraceletStyle
    {
        beads,
        paracord,
        floss
    }

    [SerializeField, Range(0.6f, 1)]
    private float captureRate;

    [SerializeField]
    private BraceletStyle style;

    [SerializeField]
    private Color mainColor;

    [SerializeField]
    private Color accentColor;

    public override bool UseItem()
    {
        return base.UseItem();
    }
}
