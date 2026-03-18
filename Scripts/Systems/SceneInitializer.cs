using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private WarpPointVariable entrancePoint;

    [SerializeField]
    private GridVariable gridRef;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObjectVariable audioRef;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameStateEvent pauzeEvent;

    [SerializeField]
    private GameObjectVariable progressionRef;

    [SerializeField]
    private ProgressionSystem progressionSystem;

    [SerializeField]
    private WarpPoint[] warpPoints;

    [SerializeField]
    private Party creatureStorage;

    [SerializeField]
    private GameObjectVariable creatureStorageRef;

    private GameObject instantiatedPlayer;

    private void Awake()
    {
        // assign system variables
        gridRef.Value = grid;
        audioRef.Value = audioSource.gameObject;
        progressionRef.Value = progressionSystem.gameObject;
        creatureStorageRef.Value = creatureStorage.gameObject;

        // get rid of ghosts
        if (playerRef.Value != null)
        {
            Destroy(playerRef.Value);
        }

        // instantiate the player and assign their variable
        if (!instantiatedPlayer)
        {
            instantiatedPlayer = Instantiate(playerPrefab);
            instantiatedPlayer.GetComponent<Party>().ID = "Player-Party";
            instantiatedPlayer.GetComponent<Mover>().ID = "Player-Mover";
            instantiatedPlayer.SetActive(true);
            playerRef.Value = instantiatedPlayer;
        }

        // send the player to the entrance point, if applicable
        if (entrancePoint.Value != null)
        {
            instantiatedPlayer.transform.position = entrancePoint.Value.Position;
        }

        pauzeEvent.Invoke(GameState.Overworld);

        foreach (WarpPoint point in warpPoints)
        {
            Debug.Log(point.SceneName);
        }
    }
}
