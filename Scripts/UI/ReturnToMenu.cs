using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField]
    public GameObjectVariable playerRef;

    public void QuitGame()
    {
        // the player can only be destroyed manually
        // Destroy(playerRef.Value);
        SceneManager.LoadScene("Start_Menu", LoadSceneMode.Single);
    }
}
