using System.Collections;
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
    private BoolEvent gamePauzedEvent;

    private bool gamePauzed = false;

    // We only want to be moving in one direction at a time, so the movement is always assigned to the same coroutine.
    private Coroutine movementCoroutine;
    private Vector3 gridDirection;
    private float gridSize = 0.32f;
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

    private void Awake()
    {
        aimLocation = transform.position;
        aimDirection = Vector3.zero;
        gamePauzedEvent.AddListener(TogglePauze);

        // to keep track of it as a saveable
        instances.Add(this);
    }

    private void TogglePauze(bool pauzed)
    {
        this.gamePauzed = pauzed;
    }

    private void OnDisable()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        gamePauzedEvent.RemoveListener(TogglePauze);
        lastPosition = transform.position;
    }

    /// <summary>
    /// Move this object in a specific direction.
    /// </summary>
    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero || gamePauzed)
        {
            return;
        }

        aimDirection = new Vector3(direction.x, direction.y);
        gridDirection = aimDirection * gridSize;
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(Movement());
    }

    // Moves the object towards direction until it's close enough to round up and jump the gap.
    private IEnumerator Movement()
    {
        Vector3 originalPosition = transform.position;
        while (Vector2.Distance(transform.position, originalPosition + gridDirection) > gapCloseDistance)
        {
            transform.Translate(speed * Time.deltaTime * gridDirection);
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition + gridDirection;

        // the aim direction is the tile past the object in the same direction it was moving
        aimLocation = transform.position + gridDirection;

        // tell any listeners that the player has moved
        movementEvent.Invoke(transform.position);
        yield return null;
    }

    /// <summary>
    /// When the object collides with a static object, do not let it continue in that direction.
    /// </summary>
    /// <param name="collision">Object that this object is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
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

        // if the last saved location was not this scene, hide this
        if (saveData.sceneName != SceneManager.GetActiveScene().name)
        {
            gameObject.SetActive(false);
            return;
        }

        // otherwise, set the position of this to its last saved position
        transform.position = saveData.position;
    }

    #endregion
}
