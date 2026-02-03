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
        entrancePoint.Value = target;
        SceneManager.LoadScene("Campgrounds", LoadSceneMode.Single);
        // StartCoroutine(LoadFirstScene());
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
