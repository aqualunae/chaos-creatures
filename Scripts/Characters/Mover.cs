using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    private Vector3 nextPosition;
    private bool isMoving = false;
    private Vector3 gridDirection;
    private Vector3 gridSize;
    private Vector3 aimLocation;
    private Vector3 aimDirection;

    private CharacterAnimator animator;

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
        isMoving = false;

        animator = GetComponentInChildren<CharacterAnimator>();

        DontDestroyOnLoad(this.gameObject);
    }

    private void TogglePauze(GameState state)
    {
        gamePauzed = state != GameState.Overworld && state != GameState.MobileOverworld;
    }

    private void OnDisable()
    {
        Stop();
        gamePauzedEvent.RemoveListener(TogglePauze);
    }

    /// <summary>
    /// Move this object once in a specific direction.
    /// </summary>
    public void Move(Vector2 direction)
    {
        if (gamePauzed)
        {
            animator?.Movement(Vector2.zero);
            return;
        }

        aimDirection = new Vector3(direction.x, direction.y);
        gridDirection = aimDirection * gridSize.x;
        nextPosition = transform.position;

        isMoving = true;
        animator?.Movement(direction);
    }

    bool inCollision = false;
    int trappedForUpdates = 0;

    private void Update()
    {
        // if the player is still moving towards the next position
        if ((stopAtNextSafePosition || isMoving) && Vector2.Distance(transform.position, nextPosition) > gapCloseDistance)
        {
            transform.Translate(speed * Time.deltaTime * gridDirection);
        }
        else if (isMoving || stopAtNextSafePosition)
        {
            // if the player is close enough to the next position
            // snap them
            transform.position = nextPosition;

            // the aim direction is the tile past the object in the same direction it was moving
            aimLocation = transform.position + gridDirection;

            // tell any listeners that the player has moved
            lastPosition = transform.position;
            movementEvent.Invoke(lastPosition);

            // since the player has moved, they're no longer at the entrance point, so it's no longer relevant
            entrancePoint.Value = null;

            // if the next position has not been calculated and the player's move keys are held
            // (aim direction is the player's movement keys)
            if (aimDirection.magnitude > 0.1f && !stopAtNextSafePosition)
            {
                
                // calculate the next position
                // the next position is always snapped to grid, as this makes interaction detection easier
                nextPosition = gridRef.Value.CellToWorld(gridRef.Value.WorldToCell(transform.position + gridDirection)) + (gridSize * 0.5f);
            }
            else
            {
                stopAtNextSafePosition = false;
                isMoving = false;
            }
        }
        else if (inCollision)
        {
            trappedForUpdates++;
            if (trappedForUpdates > 50)
            {
                EscapeTrap();
            }
        }
    }

    /// <summary>
    /// When the object collides with a static object, do not let it continue in that direction.
    /// </summary>
    /// <param name="collision">Object that this object is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        inCollision = true;
        Stop();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trappedForUpdates = 0;
        inCollision = false;
    }

    private bool stopAtNextSafePosition = false;

    public void SlowStop()
    {
        // called when the movement keys are released
        // set the movement direction to zero
        stopAtNextSafePosition = true;

        isMoving = false;
        animator?.Movement(Vector2.zero);
    }

    public void Stop()
    {
        // called on collision or disable
        // snap to the last safe position and reset the next position
        transform.position = lastPosition;
        nextPosition = lastPosition;

        isMoving = false;
        animator?.Movement(Vector2.zero);
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

    /// <summary>
    /// Checks if this object is touching any tilemap colliders.
    /// If it is, warps it to a safe point.
    /// </summary>
    private void EscapeTrap()
    {
        if (TryGetComponent<BoxCollider2D>(out var colliderSelf) && TryGetComponent<WarpSelf>(out var warpSelf))
        {
            TilemapCollider2D[] tilemapColliders = gridRef.Value.GetComponentsInChildren<TilemapCollider2D>();
            foreach (TilemapCollider2D collider in tilemapColliders)
            {
                if (collider.IsTouching(colliderSelf))
                {
                    warpSelf.WarpToTarget();
                    trappedForUpdates = 0;
                    break;
                }
            }
        }
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
