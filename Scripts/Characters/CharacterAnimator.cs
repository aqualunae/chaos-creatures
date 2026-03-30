using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.down;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Movement(Vector2 direction)
    {
        string action = "idle";

        if (direction != Vector2.zero)
        {
            lastDirection = direction;
            action = "move";
        }

        string facing = (lastDirection.x, lastDirection.y) switch
        {
            (0, 1) => "back",
            (1, 1) => "back",
            (-1, 1) => "back",
            (1, 0) => "right",
            (-1, 0) => "left",
            _ => "forward"
        };

        animator.Play($"Base.{ action }_{ facing }");
    }
}
