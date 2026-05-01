using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Mover), typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Event that is called to toggle the pauze state of the game.")]
    private GameStateEvent pauzeEvent;

    [SerializeField]
    private LayerMask interactableMask;

    [SerializeField]
    private float interactDistance = 0.32f;

    [SerializeField]
    private VoidEvent cancelEvent;

    // [SerializeField]
    // private VoidEvent nextEvent;

    private GameState gamePauzed = GameState.Overworld;

    private void Awake()
    {
        GetComponent<PlayerInput>().actions.FindActionMap("UI").Disable();
        pauzeEvent.AddListener(TogglePauze);
    }

    private void OnDisable()
    {
        pauzeEvent.RemoveListener(TogglePauze);
    }

    /// <summary>
    /// Called by the Input System to move the player.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        if (context.interaction is HoldInteraction)
        {
            if (context.canceled)
            {
                GetComponent<Mover>().SlowStop();
            }
            else if (direction != Vector2.zero)
            {
                GetComponent<Mover>().Move(direction);
            }
        }
    }

    // Call the pauze event when the pauze key is pressed.
    public void OnPauze(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // do not change the game state when the adoption window or combat window is open
            if (gamePauzed == GameState.AdoptionWindow || gamePauzed == GameState.CombatWindow)
            {
                return;
            }

            // allow exiting from the dialogue window and storage window
            if (gamePauzed == GameState.DialogueWindow)
            {
                gamePauzed = GameState.Overworld;
            }
            else if (gamePauzed == GameState.Cutscene)
            {
                gamePauzed = GameState.SkipCutscene;
            }
            // if you're in the overworld, bring up the pauze menu
            else if (gamePauzed == GameState.Overworld)
            {
                gamePauzed = GameState.PauzeMenu;
            }
            // the pauze menu and storage windows handle their own cancel/pauze input, don't interfere
            else if (gamePauzed == GameState.PauzeMenu || gamePauzed == GameState.StorageWindow)
            {
                cancelEvent.Invoke();
                return;
            }

            // trigger the event
            pauzeEvent.Invoke(gamePauzed);
        }
    }

    // Update the pauze state when the pauze event is called.
    // Keeping OnPauze and TogglePauze separate allows the key to toggle correctly even when the pauze event is called by other scripts, such as clicking resume from the pauze menu.
    private void TogglePauze(GameState state)
    {
        gamePauzed = state;
    }

    /// <summary>
    /// Called by the Input System when the player clicks.
    /// </summary>
    public void OnInteractMouse(InputAction.CallbackContext context)
    {
        //check if the game is pauzed
        if (gamePauzed != GameState.Overworld)
        {
            return;
        }

        // if the cursor is near the player
        Vector2 cursorPosition = context.ReadValue<Vector2>();
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, Camera.main.transform.position.z * -1));
        if (Vector3.Distance(targetPosition, transform.position) < interactDistance)
        {
            // try to find an interactable at the target position
            GameObject selectedObject;
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(targetPosition.x, targetPosition.y), Vector2.zero, 0, interactableMask);
            if (hitData)
            {
                selectedObject = hitData.transform.gameObject;
                // Debug.Log(selectedObject.name);
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
    public void OnInteractKey(InputAction.CallbackContext context)
    {
        //check if the game is pauzed
        if (gamePauzed != GameState.Overworld || !context.performed)
        {
            return;
        }

        Interact();
    }

    public void Interact()
    {
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
}
