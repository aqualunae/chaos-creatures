using UnityEngine;

[CreateAssetMenu(fileName = "Skill Sprites ", menuName = "Combat/Skill Sprites")]
public class SkillSprites : ScriptableObject
{
    [SerializeField]
    private Sprite[] sprites;

    public Sprite[] Sprites
    {
        get => sprites;
    }
}
