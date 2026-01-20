using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class InputHandler : MonoBehaviour
{
    private void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        if (direction != Vector2.zero)
        {
            Debug.Log(direction);
            GetComponent<Mover>().Move(direction);
        }
    }
}
