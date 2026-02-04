using Unity.VisualScripting;
using UnityEngine;

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

    private GameObject instantiatedPlayer;

    private void Awake()
    {
        gridRef.Value = grid;
        if (playerRef.Value != null)
        {
            Destroy(playerRef.Value);
        }

        if (!instantiatedPlayer)
        {
            instantiatedPlayer = Instantiate(playerPrefab);
            instantiatedPlayer.GetComponent<Party>().ID = "Player-Party";
            instantiatedPlayer.GetComponent<Mover>().ID = "Player-Mover";
            instantiatedPlayer.SetActive(true);
            playerRef.Value = instantiatedPlayer;
        }

        if (entrancePoint.Value != null)
        {
            instantiatedPlayer.transform.position = entrancePoint.Value.Position;
        }
    }
}
