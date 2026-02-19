using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mover : SaveableBehaviour
{
    [SerializeField, Tooltip("Speed of the object when in motion.")]
    private float speed;

    [SerializeField]
    private Vector3Event movementEvent;

    [SerializeField, Range(0.01f, 0.32f), Tooltip("The movement may go to an increment that isn't valid. At what distance should it jump the player to the closest valid grid location?")]
    private float gapCloseDistance = 0.1f;

    [SerializeField]
    private GameStateEvent gamePauzedEvent;

    [SerializeField]
    private WarpPointVariable entrancePoint;

    [SerializeField]
    private GridVariable gridRef;

    private bool gamePauzed = false;

    // We only want to be moving in one direction at a time, so the movement is always assigned to the same coroutine.
    private Coroutine movementCR;
    private Coroutine multiMovementCR;
    private bool moveKeyHeld = false;
    private Vector3 gridDirection;
    private Vector3 gridSize;
    private Vector3 aimLocation;
    private Vector3 aimDirection;

    public Vector3 AimLocation
    {
        get => aimLocation;
    }

    public Vector3 AimDirection
    {
        get => aimDirection;
    }

    private new void Awake()
    {
        base.Awake();

        gridSize = gridRef.Value.cellSize;
        aimLocation = transform.position;
        aimDirection = Vector3.zero;
        gamePauzedEvent.AddListener(TogglePauze);

        DontDestroyOnLoad(this.gameObject);
    }

    private void TogglePauze(GameState state)
    {
        gamePauzed = state != GameState.Overworld;
    }

    private void OnDisable()
    {
        if (movementCR != null)
        {
            StopCoroutine(movementCR);
        }
        gamePauzedEvent.RemoveListener(TogglePauze);
    }

    /// <summary>
    /// Move this object once in a specific direction.
    /// </summary>
    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero || gamePauzed)
        {
            return;
        }

        aimDirection = new Vector3(direction.x, direction.y);
        gridDirection = aimDirection * gridSize.x;
        if (movementCR != null)
        {
            StopCoroutine(movementCR);
        }
        movementCR = StartCoroutine(SingleMovement());
    }

    // Moves the object towards direction until it's close enough to round up and jump the gap.
    private IEnumerator SingleMovement()
    {
        Vector3 originalPosition = transform.position;
        Vector3 snappedPosition = gridRef.Value.CellToWorld(gridRef.Value.WorldToCell(originalPosition + gridDirection)) + (gridSize * 0.5f);
        while (Vector2.Distance(transform.position, snappedPosition) > gapCloseDistance)
        {
            transform.Translate(speed * Time.deltaTime * gridDirection);
            yield return new WaitForEndOfFrame();
        }
        
        transform.position = snappedPosition;

        // the aim direction is the tile past the object in the same direction it was moving
        aimLocation = transform.position + gridDirection;

        // tell any listeners that the player has moved
        lastPosition = transform.position;
        movementEvent.Invoke(lastPosition);

        // since the player has moved, they're no longer at the entrance point, so it's no longer relevant
        entrancePoint.Value = null;
        yield return null;
    }

    public void MoveContinuous(Vector2 direction)
    {
        if (direction == Vector2.zero || gamePauzed)
        {
            return;
        }

        aimDirection = new Vector3(direction.x, direction.y);
        gridDirection = aimDirection * gridSize.x;
        if (movementCR != null)
        {
            StopCoroutine(movementCR);
        }
        moveKeyHeld = true;
        multiMovementCR = StartCoroutine(MultiMovement());
    }

    private IEnumerator MultiMovement()
    {
        int safetyCounter = 50;
        do
        {
            yield return movementCR = StartCoroutine(SingleMovement());
            safetyCounter--;
        }
        while (moveKeyHeld && safetyCounter > 0);
        yield return null;
    }

    /// <summary>
    /// When the object collides with a static object, do not let it continue in that direction.
    /// </summary>
    /// <param name="collision">Object that this object is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
    }

    public void SlowStop()
    {
        moveKeyHeld = false;
    }

    public void Stop()
    {
        moveKeyHeld = false;
        if (multiMovementCR != null)
        {
            StopCoroutine(multiMovementCR);
        }
        if (movementCR != null)
        {
            StopCoroutine(movementCR);
        }
        transform.position = lastPosition;
    }

    #region Saving

    public class MoverSaveData
    {
        public Vector3 position;
        public string sceneName;
    }

    private Vector3 lastPosition;

    public override void OnNewGame()
    {
        // doesn't actually have data to initialize here
    }

    public override Saveable OnSave()
    {
        MoverSaveData saveData = new MoverSaveData()
        {
            position = lastPosition,
            sceneName = SceneManager.GetActiveScene().name
        };

        string data = JsonUtility.ToJson(saveData);
        string identifier = $"{typeof(Mover)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        MoverSaveData saveData = JsonUtility.FromJson<MoverSaveData>(saveable.data);

        // unused code for non-player moving objects
        // if the last saved location was not this scene, hide this
        // if (saveData.sceneName != SceneManager.GetActiveScene().name)
        // {
        //     gameObject.SetActive(false);
        //     return;
        // }

        if (transform.IsDestroyed())
        {
            return;
        }

        // if the entrancePoint is set, you're entering a scene from a warp point
        // which means your saveData.position is the exit point of the previous map, not the entrance point of the current map
        // so you need to set your position to the entrance point
        if (entrancePoint.Value != null)
        {
            transform.position = entrancePoint.Value.Position;
        }
        else
        {
            // otherwise, set the position of this to its last saved position
            transform.position = saveData.position;
        }

        lastPosition = transform.position;
    }

    #endregion
}
