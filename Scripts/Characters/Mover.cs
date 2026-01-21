using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the object when in motion.")]
    private float speed;

    // We only want to be moving in one direction at a time, so the movement is always assigned to the same coroutine.
    private Coroutine movementCoroutine;
    private Vector3 direction3;

    /// <summary>
    /// Move this object in a specific direction.
    /// </summary>
    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        direction3 = new Vector3(direction.x, direction.y) * 0.32f;
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(Movement());
    }

    // Moves the object towards direction until it's close enough to round up and jump the gap.
    private IEnumerator Movement()
    {
        Vector3 originalPosition = transform.position;
        while (Vector2.Distance(transform.position, originalPosition + direction3) > 0.1f)
        {
            transform.Translate(speed * Time.deltaTime * direction3);
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition + direction3;
        yield return null;
    }

    /// <summary>
    /// When the object collides with a static object, do not let it continue in that direction.
    /// </summary>
    /// <param name="collision">Object that this object is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(movementCoroutine);
        // Right now, this disables movement completely when collision occurs, because the object does not bounce back from the collision.
        // Logic options:
        // - Disable that direction only until the object has moved in a different direction.
        // - Put collision on a cooldown. Could enable glitching through things?
    }
}
