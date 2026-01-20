using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private string filename;

    [System.Serializable]
    private class SaveMaster
    {
        public List<SaveableBehaviour.Saveable> saveables;
    }

    private SaveMaster saveMaster;

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
        StreamWriter writer = new StreamWriter(Path.Combine(Application.persistentDataPath, filename));
        writer.Write(saveData);
        writer.Close();
    }

    public void OnLoad()
    {
        StreamReader reader = new StreamReader(Path.Combine(Application.persistentDataPath, filename));
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

    private void Start()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, filename)))
        {
            OnLoad();
        }
    }

    private void OnDisable()
    {
        OnSave();
    }
}
