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

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip combatMusic;

    private AudioClip overworldMusic;
    private GameState gameState;

    private void Awake()
    {
        pauzeEvent.AddListener(TogglePauzeByListener);
        overworldMusic = audioSource.clip;
    }

    // Only allow the pauze state to be toggled by the listener. Otherwise it results in unpredictable behaviour.
    private void TogglePauzeByListener(GameState state)
    {
        if (state == GameState.Overworld)
        {
            // in the overworld, use the player action map and disable the pauze menu
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            pauzeMenu.SetActive(false);
        }
        else 
        {
            // in all other states, use the ui action map
            playerRef.Value.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

            // enable the pauze menu if needed
            if (state == GameState.PauzeMenu)
            {
                pauzeMenu.SetActive(true);
            }
        }

        // if the game state is currently combat
        if (state == GameState.CombatWindow)
        {
            audioSource.clip = combatMusic;
            audioSource.Play();
        }
        // if the most recent gamestate was combat
        else if (gameState == GameState.CombatWindow)
        {
            audioSource.clip = overworldMusic;
            audioSource.Play();
        }
        
        // save the current game state so it can be compared to the next game state
        gameState = state;
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
