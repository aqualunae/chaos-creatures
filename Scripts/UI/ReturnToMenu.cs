using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void QuitGame()
    {
        SceneManager.LoadScene("Start_Menu", LoadSceneMode.Single);
    }
}
