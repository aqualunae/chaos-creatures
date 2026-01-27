using UnityEngine;
using UnityEngine.SceneManagement;

public class Warper : MonoBehaviour
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
