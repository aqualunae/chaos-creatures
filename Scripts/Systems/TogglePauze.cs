using UnityEngine;

/// <summary>
/// Used on the Resume button of the pauze menu.
/// For the system that handles pauzing, see PauzeGame.
/// </summary>
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
