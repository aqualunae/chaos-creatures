using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BraceletRenderer : MonoBehaviour
{
    [System.Serializable]
    public class BraceletSprites
    {
        public Bracelet.BraceletStyle braceletStyle;
        public Sprite baseSprite;
        public Sprite accentSprite;
    }

    [SerializeField]
    private Image baseSprite;

    [SerializeField]
    private Image accentSprite;

    [SerializeField]
    private BraceletSprites[] sprites;

    [SerializeField]
    private ColorListVariable colorList;

    public void Initialize(Bracelet.BraceletStyle style, ColorTitle baseColor, ColorTitle accentColor)
    {
        BraceletSprites braceletSprites = sprites.First(bsp => bsp.braceletStyle == style);
        baseSprite.sprite = braceletSprites.baseSprite;
        accentSprite.sprite = braceletSprites.accentSprite;
        baseSprite.color = colorList.GetColor(baseColor);
        accentSprite.color = colorList.GetColor(accentColor);
    }
}
