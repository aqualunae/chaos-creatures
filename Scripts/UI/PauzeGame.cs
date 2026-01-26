using UnityEngine;

public class PauzeGame : MonoBehaviour
{
    [SerializeField]
    private BoolEvent gamePauzedEvent;

    [SerializeField]
    private GameObject pauzeMenu;

    private void Awake()
    {
        gamePauzedEvent.AddListener(TogglePauzeByListener);
    }

    // Only allow the pauze state to be toggled by the listener. Otherwise it results in unpredictable behaviour.
    private void TogglePauzeByListener(bool pauzed)
    {
        if (pauzed)
        {
            pauzeMenu.SetActive(true);
        }
        else
        {
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
