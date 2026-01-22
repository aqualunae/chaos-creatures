using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Name of the file to be saved. Do not include extension or path.")]
    private string filename;

    [System.Serializable]
    private class SaveMaster
    {
        public List<SaveableBehaviour.Saveable> saveables;
    }

    private SaveMaster saveMaster;

    /// <summary>
    /// Combines the persistent data path, filename, and JSON extension.
    /// </summary>
    /// <returns>Full filepath of the save data.</returns>
    private string GetSavePath()
    {
        return $"{Path.Combine(Application.persistentDataPath, filename)}.json";
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
        StreamWriter writer = new StreamWriter(GetSavePath());
        writer.Write(saveData);
        writer.Close();
    }

    /// <summary>
    /// Reads save data from the file and distributes it to all SaveableBehaviours that have added themselves to the static Instances HashSet. If there's no data for a specific object, ask it to initialize its data.
    /// </summary>
    public void OnLoad()
    {
        StreamReader reader = new StreamReader(GetSavePath());
        string jsonSaveData = reader.ReadToEnd();
        saveMaster = JsonUtility.FromJson<SaveMaster>(jsonSaveData);
        HashSet<SaveableBehaviour> saveableBehaviors = SaveableBehaviour.Instances;
        foreach(SaveableBehaviour saveableBehavior in saveableBehaviors)
        {
            if (saveMaster.saveables.Exists(saveable => saveable.id.Contains(saveableBehavior.ID)))
            {
                SaveableBehaviour.Saveable saveData = saveMaster.saveables.Find(saveable => saveable.id.Contains(saveableBehavior.ID));
                saveableBehavior.OnLoad(saveData);
            }
            else
            {
                saveableBehavior.OnNewGame();
            }
        }
        reader.Close();
    }

    /// <summary>
    /// OnLoad needs to be placed in Start because it needs to be called after the Instances HashSet is filled, which happens on Awake.
    /// </summary>
    private void Start()
    {
        if (File.Exists(GetSavePath()))
        {
            OnLoad();
        }
        else
        {
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
