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

    public void TogglePauze(bool pauzed)
    {
        gamePauzedEvent.Invoke(pauzed);
    }

    private void OnDisable()
    {
        gamePauzedEvent.RemoveListener(TogglePauzeByListener);
    }
}
