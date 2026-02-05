using UnityEngine;
using UnityEngine.InputSystem;

public class PauzeGame : MonoBehaviour
{
    [SerializeField]
    private BoolEvent gamePauzedEvent;

    [SerializeField]
    private GameObject pauzeMenu;

    [SerializeField]
    private GameObjectVariable playerRef;

    private void Awake()
    {
        gamePauzedEvent.AddListener(TogglePauzeByListener);
    }

    // Only allow the pauze state to be toggled by the listener. Otherwise it results in unpredictable behaviour.
    private void TogglePauzeByListener(bool pauzed)
    {
        if (pauzed)
        {
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            pauzeMenu.SetActive(true);
        }
        else
        {
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            pauzeMenu.SetActive(false);
        }
    }

    // If the pauze state needs to be toggled, invoke the listener.
    public void TogglePauze(bool pauzed)
    {
        gamePauzedEvent.Invoke(pauzed);
    }

    private void OnDisable()
    {
        gamePauzedEvent.RemoveListener(TogglePauzeByListener);
    }
}
