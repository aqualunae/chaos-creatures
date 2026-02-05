using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class InputHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Bool Event that is called to toggle the pauze state of the game.")]
    private BoolEvent gamePauzedEvent;

    [SerializeField]
    private LayerMask interactableMask;

    [SerializeField]
    private float interactDistance = 0.32f;

    [SerializeField]
    private VoidEvent nextEvent;

    private bool gamePauzed = false;

    private void Awake()
    {
        gamePauzedEvent.AddListener(TogglePauze);
    }

    private void OnDisable()
    {
        gamePauzedEvent.RemoveListener(TogglePauze);
    }

    /// <summary>
    /// Called by the Input System to move the player.
    /// </summary>
    private void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        if (direction != Vector2.zero)
        {
            GetComponent<Mover>().Move(direction);
        }
    }

    // Call the pauze event when the pauze key is pressed.
    private void OnPauze(InputValue value)
    {
        gamePauzedEvent.Invoke(!gamePauzed);
    }

    // Update the pauze state when the pauze event is called.
    // Keeping OnPauze and TogglePauze separate allows the key to toggle correctly even when the pauze event is called by other scripts, such as clicking resume from the pauze menu.
    private void TogglePauze(bool pauzed)
    {
        gamePauzed = pauzed;
    }

    /// <summary>
    /// Called by the Input System when the player clicks.
    /// </summary>
    private void OnInteractMouse(InputValue value)
    {
        //check if the game is pauzed
        if (gamePauzed)
        {
            return;
        }

        // if the cursor is near the player
        Vector2 cursorPosition = value.Get<Vector2>();
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, Camera.main.transform.position.z * -1));
        if (Vector3.Distance(targetPosition, transform.position) < interactDistance)
        {
            Debug.Log(targetPosition);
            // try to find an interactable at the target position
            GameObject selectedObject;
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(targetPosition.x, targetPosition.y), Vector2.zero, 0, interactableMask);
            if (hitData)
            {
                selectedObject = hitData.transform.gameObject;
                if (selectedObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    // if an interactable was found, invoke its interaction
                    interactable.interactAction.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Called by the Input System when the player presses their Interact key or button.
    /// </summary>
    private void OnInteractKey(InputValue value)
    {
        //check if the game is pauzed
        if (gamePauzed)
        {
            return;
        }

        // get the player's facing direction
        Vector3 direction = GetComponent<Mover>().AimDirection;

        // try to find an interactable in that direction
        GameObject selectedObject;
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, interactDistance, interactableMask);
        if (hitData)
        {
            selectedObject = hitData.transform.gameObject;
            if (selectedObject.TryGetComponent<Interactable>(out Interactable interactable))
            {
                // if an interactable was found, invoke its interaction
                interactable.interactAction.Invoke();
            }
        }
    }

    private void OnNext(InputValue value)
    {
        nextEvent.Invoke();
    }
}
