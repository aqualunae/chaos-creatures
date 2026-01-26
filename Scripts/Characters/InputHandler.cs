using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class InputHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Bool Event that is called to toggle the pauze state of the game.")]
    private BoolEvent gamePauzedEvent;

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
    /// Called by the Input System
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

    private void OnInteract(InputValue value)
    {
        // Put some logic here to handle interacting with NPCs and looting objects.
    }
}
