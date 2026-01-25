using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadGame()
    {
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
