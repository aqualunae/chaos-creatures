using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Event called when something happens to progress the game.")]
    private SaveEvent trigger;

    [System.Serializable]
    public class SaveMaster
    {
        public List<SaveableBehaviour.Saveable> saveables;
    }

    private SaveMaster saveMaster;
    private static readonly string filename = "ccdata";

    /// <summary>
    /// Combines the persistent data path, filename, and JSON extension.
    /// </summary>
    /// <returns>Full filepath of the save data.</returns>
    public static string GetSavePath()
    {
        string savePath = Application.persistentDataPath;

        #if UNITY_WEBGL
        savePath = "idbfs/Chaos_Creatures_Data";
        #endif

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string filepath = $"{Path.Combine(savePath, filename)}.txt";
        Debug.Log(filepath);

        // if (!File.Exists(filepath))
        // {
        //     File.CreateText(filepath);
        // }

        return filepath;
    }

    /// <summary>
    /// Requests save data from all SaveableBehaviours that have added themselves to the static Instances HashSet, and writes it to file.
    /// </summary>
    public void OnSave()
    {
        List<SaveableBehaviour.Saveable> saveables = new List<SaveableBehaviour.Saveable>();
        HashSet<SaveableBehaviour> saveableBehaviors = SaveableBehaviour.Instances;
        foreach(SaveableBehaviour saveableBehavior in saveableBehaviors)
        {
            saveables.Add(saveableBehavior.OnSave());
        }
        SaveMaster saveMaster = new SaveMaster()
        {
            saveables = saveables
        };
        string saveData = JsonUtility.ToJson(saveMaster);

        File.WriteAllText(GetSavePath(), saveData);

        // StreamWriter writer = new StreamWriter(GetSavePath());
        // writer.Write(saveData);
        // writer.Close();
    }

    /// <summary>
    /// Reads save data from the file and distributes it to all SaveableBehaviours that have added themselves to the static Instances HashSet. If there's no data for a specific object, ask it to initialize its data.
    /// </summary>
    public void OnLoad()
    {
        // StreamReader reader = new StreamReader(GetSavePath());
        // string jsonSaveData = reader.ReadToEnd();
        string jsonSaveData = File.ReadAllText(GetSavePath());
        saveMaster = JsonUtility.FromJson<SaveMaster>(jsonSaveData);
        HashSet<SaveableBehaviour> saveableBehaviors = SaveableBehaviour.Instances;
        foreach(SaveableBehaviour saveableBehavior in saveableBehaviors)
        {
            if (saveMaster.saveables.Exists(saveable => saveable.id.Contains(saveableBehavior.ID)))
            {
                SaveableBehaviour.Saveable saveData = saveMaster.saveables.Find(saveable => saveable.id.Contains(saveableBehavior.ID));
                if (saveableBehavior)
                {
                    saveableBehavior.OnLoad(saveData);
                }
            }
            else
            {
                saveableBehavior.OnNewGame();
            }
        }
        // reader.Close();
    }

    private void Awake()
    {
        trigger.AddListener(HandleTrigger);
    }

    private void HandleTrigger(SaveState state)
    {
        if (state == SaveState.Save)
        {
            OnSave();
        }
    }

    /// <summary>
    /// OnLoad needs to be placed in Start because it needs to be called after the Instances HashSet is filled, which happens on Awake.
    /// </summary>
    private void Start()
    {
        if (File.Exists(GetSavePath()))
        {
            trigger.Invoke(SaveState.Load);
            OnLoad();
        }
        else
        {
            trigger.Invoke(SaveState.NewGame);
            HashSet<SaveableBehaviour> saveableBehaviors = SaveableBehaviour.Instances;
            foreach(SaveableBehaviour saveableBehavior in saveableBehaviors)
            {
                saveableBehavior.OnNewGame();
            }
        }
    }

    /// <summary>
    /// Save the game when the Save System is disabled, usually on quit.
    /// </summary>
    private void OnDisable()
    {
        OnSave();
    }
}
