using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class InputHandler : MonoBehaviour
{
    /// <summary>
    /// Called by the Input System
    /// </summary>
    private void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        if (direction != Vector2.zero)
        {
            Debug.Log(direction);
            GetComponent<Mover>().Move(direction);
        }
    }

    private void OnInteract(InputValue value)
    {
        // Put some logic here to handle interacting with NPCs and loot objects.
    }
}
