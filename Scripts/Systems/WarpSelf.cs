using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSelf : MonoBehaviour
{
    [SerializeField]
    private WarpPoint target;

    [SerializeField, Tooltip("Variable that tells the player where to load when entering a new scene.")]
    private WarpPointVariable entrancePoint;

    public void WarpToTarget()
    {
        if (SceneManager.GetActiveScene().name == target.SceneName)
        {
            transform.position = target.Position;
        }
        else
        {
            // set the entrance point to the target and load that scene
            entrancePoint.Value = target;
            SceneManager.LoadScene(target.SceneName, LoadSceneMode.Single);
        }
    }
}
