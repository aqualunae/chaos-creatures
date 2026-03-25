using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(InputSystemUIInputModule))]
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

    private void Awake()
    {
        // dispose of ghosts
        if (playerRef.Value != null)
        {
            playerRef.Value.SetActive(false);
        }

        GetComponent<InputSystemUIInputModule>().actionsAsset.actionMaps[0].Disable();
        GetComponent<InputSystemUIInputModule>().actionsAsset.actionMaps[1].Enable();

        // set the load game button to disabled if there is no save data
        loadGame.interactable = File.Exists(SaveSystem.GetSavePath());
    }

    public void LoadGame()
    {
        // find and load the player's current scene
        StreamReader reader = new StreamReader(SaveSystem.GetSavePath());
        string jsonSaveData = reader.ReadToEnd();
        SaveSystem.SaveMaster saveMaster = JsonUtility.FromJson<SaveSystem.SaveMaster>(jsonSaveData);
        string playerSceneName = JsonUtility.FromJson<Mover.MoverSaveData>(saveMaster.saveables.Find(saveable => saveable.id.Contains("Player-Mover")).data).sceneName;

        SceneManager.LoadScene(playerSceneName, LoadSceneMode.Single);
    }

    public void NewGame()
    {
        // get rid of previous save data
        if (File.Exists(SaveSystem.GetSavePath()))
        {
            // todo: ask for confirmation
            // todo: multiple save slots
            File.Delete(SaveSystem.GetSavePath());
        }

        // initialize the player at the campgrounds entrance point
        entrancePoint.Value = target;
        SceneManager.LoadScene("Campgrounds", LoadSceneMode.Single);
    }
}