using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteSwapper : MonoBehaviour
{
    public Sprite[] sprites;

    [SerializeField]
    private SpriteRenderer target;

    private void LateUpdate()
    {
        if (target.sprite != null && int.TryParse(target.sprite.name.Split("_")[^1], out int frame))
        {
            if (sprites.Length > frame)
            {
                target.sprite = sprites[frame];
            }
            else
            {
                target.sprite = null;
            }
        }
    }
}
