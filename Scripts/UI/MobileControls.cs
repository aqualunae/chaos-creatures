using UnityEngine;

public class MobileControls : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private GameStateEvent pauzeEvent;

    private Mover mover;
    private InputHandler input;

    private bool mobileEnabled;

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
        mover = playerRef.Value.GetComponent<Mover>();
        input = playerRef.Value.GetComponent<InputHandler>();
    }

    private void MobileOverworld(GameState state)
    {
        if (state == GameState.Overworld)
        {
            pauzeEvent.Invoke(GameState.MobileOverworld);
        }
        else if (state == GameState.MobileOverworld)
        {
            mobileEnabled = true;
        }
        else
        {
            mobileEnabled = false;
            mover.SlowStop();
        }
    }

    public void Pauze()
    {
        if (!mobileEnabled) { return; }
        pauzeEvent.Invoke(GameState.PauzeMenu);
    }

    public void Interact()
    {
        if (!mobileEnabled) { return; }
        input.Interact();
    }

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

    public void Stop()
    {
        if (!mobileEnabled) { return; }
        mover.SlowStop();
    }
}
