using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private GameStateEvent pauzeEvent;

    private Mover mover;
    private InputHandler input;

    private bool mobileEnabled;
    private Button[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    /// <summary>
    /// Enable or disable mobile controls and their listeners.
    /// </summary>
    /// <param name="state">True for enabled</param>
    public void EnableMobile(bool state)
    {
        if (state)
        {
            pauzeEvent.AddListener(MobileOverworld);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        pauzeEvent.RemoveListener(MobileOverworld);
    }

    private void OnEnable()
    {
        // find player components
        mover = playerRef.Value.GetComponent<Mover>();
        input = playerRef.Value.GetComponent<InputHandler>();
    }

    /// <summary>
    /// Listens for GameState events. Toggles mobile controls temporarily based on pauze state.
    /// </summary>
    /// <param name="state"></param>
    private void MobileOverworld(GameState state)
    {
        // if a different object has set the game state to overworld, change it to mobile overworld
        // this enables the ui control mapping
        if (state == GameState.Overworld)
        {
            pauzeEvent.Invoke(GameState.MobileOverworld);
        }
        else if (state == GameState.MobileOverworld)
        {
            // if we have just set the game state to mobile overworld, enable buttons and set them to active
            mobileEnabled = true;
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            // if we're in a game state other than mobile overworld, hide the mobile controls
            mobileEnabled = false;
            mover.SlowStop();
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    // pauze the game
    public void Pauze()
    {
        if (!mobileEnabled) { return; }
        pauzeEvent.Invoke(GameState.PauzeMenu);
    }

    // interact with something
    public void Interact()
    {
        if (!mobileEnabled) { return; }
        input.Interact();
    }

    // movement
    public void Up()
    {
        if (!mobileEnabled) { return; }
        mover.Move(Vector2.up);
    }

    public void Down()
    {
        if (!mobileEnabled) { return; }
        mover.Move(Vector2.down);
    }

    public void Left()
    {
        if (!mobileEnabled) { return; }
        mover.Move(Vector2.left);
    }

    public void Right()
    {
        if (!mobileEnabled) { return; }
        mover.Move(Vector2.right);
    }

    // stop moving
    public void Stop()
    {
        if (!mobileEnabled) { return; }
        mover.SlowStop();
    }
}
