using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpPointAssignment : MonoBehaviour
{
    [SerializeField]
    private WarpPoint warpPoint;

    private void Awake()
    {
        warpPoint.Position = transform.position;
        warpPoint.SceneName = SceneManager.GetActiveScene().name;
    }
}
