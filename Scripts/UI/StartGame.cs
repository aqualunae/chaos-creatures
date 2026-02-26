using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private WarpPoint target;

    [SerializeField]
    private WarpPointVariable entrancePoint;

    public void LoadGame()
    {
        if (!string.IsNullOrEmpty(target.SceneName))
        {
            entrancePoint.Value = target;
            SceneManager.LoadScene("Campgrounds", LoadSceneMode.Single);
        }
    }

    private IEnumerator LoadFirstScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Campgrounds");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
