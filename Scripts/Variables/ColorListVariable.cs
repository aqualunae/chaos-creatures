using System.Linq;
using UnityEngine;

public enum ColorTitle
{
    Red, 
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Pink,
    Black, 
    White
}

[System.Serializable]
public class ColorVariable
{
    public Color color;
    public ColorTitle title;
}

[CreateAssetMenu(fileName = "Color List", menuName = "Variables/Color List")]
public class ColorListVariable : Variable<ColorVariable[]>
{
    public Color GetColor(ColorTitle colorTitle)
    {
        return Value.First(color => color.title == colorTitle).color;
    }

    public ColorVariable GetColor(int index)
    {
        return Value[index];
    }
}
