using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class WarpPointAssignment : MonoBehaviour
{
    [SerializeField]
    private WarpPoint warpPoint;

    [SerializeField]
    private WarpPoint target;

    [SerializeField]
    private WarpPointVariable entrancePoint;

    [SerializeField, Tooltip("Set the exit point outside of the collider so that the warps don't ricochet.")]
    private GameObject exitPoint;

    // start not awake so that the scene is loaded before this is called
    private void Start()
    {
        warpPoint.Position = exitPoint.transform.position;
        warpPoint.SceneName = gameObject.scene.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target && collision.gameObject.TryGetComponent<Mover>(out Mover mover))
        {
            WarpToTarget(mover);
        }
    }

    private void WarpToTarget(Mover mover)
    {
        if (mover.TryGetComponent<PlayerInput>(out PlayerInput player))
        {
            if (!target.SceneName.Equals(SceneManager.GetActiveScene().name))
            {
                entrancePoint.Value = target;
                SceneManager.LoadScene(target.SceneName, LoadSceneMode.Single);
            }
            else
            {
                mover.transform.position = target.Position;
            }
        }
    }
}
