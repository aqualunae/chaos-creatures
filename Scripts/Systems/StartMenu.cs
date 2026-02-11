using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private WarpPoint target;

    [SerializeField]
    private WarpPointVariable entrancePoint;

    [SerializeField, Tooltip("Name of the file to be saved. Do not include extension or path.")]
    private StringVariable filename;

    [SerializeField]
    private Button loadGame;

    /// <summary>
    /// Combines the persistent data path, filename, and JSON extension.
    /// </summary>
    /// <returns>Full filepath of the save data.</returns>
    private string GetSavePath()
    {
        return $"{Path.Combine(Application.persistentDataPath, filename.Value)}.json";
    }

    private void Awake()
    {
        // dispose of ghosts
        if (playerRef.Value != null)
        {
            playerRef.Value.SetActive(false);
        }

        // set the load game button to disabled if there is no save data
        loadGame.interactable = File.Exists(GetSavePath());
    }

    public void LoadGame()
    {
        // find and load the player's current scene
        StreamReader reader = new StreamReader(GetSavePath());
        string jsonSaveData = reader.ReadToEnd();
        SaveSystem.SaveMaster saveMaster = JsonUtility.FromJson<SaveSystem.SaveMaster>(jsonSaveData);
        string playerSceneName = JsonUtility.FromJson<Mover.MoverSaveData>(saveMaster.saveables.Find(saveable => saveable.id.Contains("Player-Mover")).data).sceneName;

        SceneManager.LoadScene(playerSceneName, LoadSceneMode.Single);
    }

    public void NewGame()
    {
        // get rid of previous save data
        if (File.Exists(GetSavePath()))
        {
            // todo: ask for confirmation
            // todo: multiple save slots
            File.Delete(GetSavePath());
        }

        // initialize the player at the campgrounds entrance point
        entrancePoint.Value = target;
        SceneManager.LoadScene("Campgrounds", LoadSceneMode.Single);
    }
}