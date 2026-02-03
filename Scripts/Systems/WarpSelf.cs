using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSelf : MonoBehaviour
{
    [SerializeField]
    private WarpPoint target;

    public void WarpToTarget()
    {
        if (SceneManager.GetActiveScene().name == target.SceneName)
        {
            transform.position = target.Position;
        }
    }
}
