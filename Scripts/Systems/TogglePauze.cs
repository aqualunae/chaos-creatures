using UnityEngine;

public class TogglePauze : MonoBehaviour
{
    [SerializeField]
    private GameStateEvent pauzeEvent;

    public void ShowPauzeMenu(bool pauzed)
    {
        GameState state = pauzed ? GameState.PauzeMenu : GameState.Overworld;
        pauzeEvent.Invoke(state);
    }
}
