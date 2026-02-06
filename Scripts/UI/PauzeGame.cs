using UnityEngine;
using UnityEngine.InputSystem;

public class PauzeGame : MonoBehaviour
{
    [SerializeField]
    private GameStateEvent pauzeEvent;

    [SerializeField]
    private GameObject pauzeMenu;

    [SerializeField]
    private GameObjectVariable playerRef;

    private void Awake()
    {
        pauzeEvent.AddListener(TogglePauzeByListener);
    }

    // Only allow the pauze state to be toggled by the listener. Otherwise it results in unpredictable behaviour.
    private void TogglePauzeByListener(GameState state)
    {
        if (state == GameState.Overworld)
        {
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            pauzeMenu.SetActive(false);
        }
        else 
        {
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            if (state == GameState.PauzeMenu)
            {
                pauzeMenu.SetActive(true);
            }
        }
    }

    // If the pauze state needs to be toggled, invoke the listener.
    public void TogglePauze(GameState state)
    {
        pauzeEvent.Invoke(state);
    }

    private void OnDisable()
    {
        pauzeEvent.RemoveListener(TogglePauzeByListener);
    }
}
