using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CutscenePlayer))]
public class CutsceneSaver : SaveableBehaviour
{
    [SerializeField, Tooltip("List of cutscenes for this scene")]
    private Cutscene[] cutscenes;

    // title and toggle when a cutscene has been played
    private Dictionary<string, bool> cutsceneHistory;

    public void PlayCutscene(int index)
    {
        // if the index is out of bounds
        // or the cutscene has already played
        // do nothing
        if (index >= cutscenes.Length || cutsceneHistory[cutscenes[index].Title])
        {
            return;
        }

        // find and play the cutscene
        Cutscene cutscene = cutscenes[index];
        gameObject.SetActive(true);
        GetComponent<CutscenePlayer>().PlayCutscene(cutscene);
    }

    #region Saving

    
    public class CutsceneSaveData
    {
        public string title;
        public bool hasPlayed;
    }

    [System.Serializable]
    public class CutscenePlayerSaveData
    {
        public List<CutsceneSaveData> cutsceneHistory;
    }

    public override void OnNewGame()
    {
        cutsceneHistory = new Dictionary<string, bool>();
        foreach (Cutscene cutscene in cutscenes)
        {
            cutsceneHistory.Add(cutscene.Title, false);
        }
    }

    public override Saveable OnSave()
    {
        List<CutsceneSaveData> savedCutscenes = new List<CutsceneSaveData>();
        foreach (KeyValuePair<string, bool> cutscene in cutsceneHistory)
        {
            savedCutscenes.Add(new CutsceneSaveData()
            {
                title = cutscene.Key,
                hasPlayed = cutscene.Value
            });
        }

        CutscenePlayerSaveData saveData = new CutscenePlayerSaveData()
        {
            cutsceneHistory = savedCutscenes
        };

        string data = JsonUtility.ToJson(saveData);

        string identifier = $"{typeof(CutscenePlayer)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        cutsceneHistory = new Dictionary<string, bool>();
        CutscenePlayerSaveData saveData = JsonUtility.FromJson<CutscenePlayerSaveData>(saveable.data);
        foreach (CutsceneSaveData cutscene in saveData.cutsceneHistory)
        {
            cutsceneHistory.Add(cutscene.title, cutscene.hasPlayed);
        }
    }

    #endregion
}
