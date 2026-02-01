using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the object when in motion.")]
    private float speed;

    [SerializeField]
    private Vector3Event movementEvent;

    [SerializeField, Range(0.01f, 0.32f), Tooltip("The movement may go to an increment that isn't valid. At what distance should it jump the player to the closest valid grid location?")]
    private float gapCloseDistance = 0.1f;

    [SerializeField]
    private BoolEvent gamePauzedEvent;

    private bool gamePauzed = false;

    // We only want to be moving in one direction at a time, so the movement is always assigned to the same coroutine.
    private Coroutine movementCoroutine;
    private Vector3 gridDirection;
    private float gridSize = 0.32f;
    private Vector3 aimLocation;
    private Vector3 aimDirection;

    public Vector3 AimLocation
    {
        get => aimLocation;
    }

    public Vector3 AimDirection
    {
        get => aimDirection;
    }

    private void Awake()
    {
        aimLocation = transform.position;
        aimDirection = Vector3.zero;
        gamePauzedEvent.AddListener(TogglePauze);
    }

    private void TogglePauze(bool pauzed)
    {
        this.gamePauzed = pauzed;
    }

    private void OnDisable()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        gamePauzedEvent.RemoveListener(TogglePauze);
    }

    /// <summary>
    /// Move this object in a specific direction.
    /// </summary>
    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero || gamePauzed)
        {
            return;
        }

        aimDirection = new Vector3(direction.x, direction.y);
        gridDirection = aimDirection * gridSize;
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
        while (Vector2.Distance(transform.position, originalPosition + gridDirection) > gapCloseDistance)
        {
            transform.Translate(speed * Time.deltaTime * gridDirection);
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition + gridDirection;

        // the aim direction is the tile past the object in the same direction it was moving
        aimLocation = transform.position + gridDirection;

        // tell any listeners that the player has moved
        movementEvent.Invoke(transform.position);
        yield return null;
    }

    /// <summary>
    /// When the object collides with a static object, do not let it continue in that direction.
    /// </summary>
    /// <param name="collision">Object that this object is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(movementCoroutine);
    }
}
