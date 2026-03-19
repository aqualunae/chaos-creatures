using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressionSystem : SaveableBehaviour
{
    [SerializeField, Tooltip("Progression flags; titles of milestones or achievements.")]
    private string[] trackedFlags;

    [SerializeField, Tooltip("Event through which progression flags are fired.")]
    private StringEvent trigger;

    // internal list of whether tracked flags have fired
    private List<bool> flags;

    [System.Serializable]
    public class ProgressionStats
    {
        public int victoryCount;
        public int defeatCount;
    }

    // player stats
    private ProgressionStats stats;

    private void Start()
    {
        trigger.AddListener(EvaluateTrigger);
    }

    /// <summary>
    /// When a progression event occurs, update the relevant flag.
    /// </summary>
    /// <param name="trigger">Progression flag that was triggered.</param>
    private void EvaluateTrigger(string trigger)
    {
        Debug.Log(trigger);
        for (int i = 0; i < trackedFlags.Length; i++)
        {
            if (trackedFlags[i].Equals(trigger))
            {
                flags[i] = true;
            }
        }

        if (trigger.Contains("Victory"))
        {
            stats.victoryCount++;
        }
        else if (trigger.Equals("Defeat"))
        {
            stats.defeatCount++;
        }
    }

    /// <summary>
    /// Used by external components to ask the Progression System whether flags have been triggered.
    /// </summary>
    /// <param name="flag">The progression flag to check.</param>
    /// <returns>Whether the flag has been triggered or not.</returns>
    public bool CheckFlag(string flag)
    {
        if (flags == null)
        {
            Debug.Log("No flags!");
            return false;
        }
        
        for (int i = 0; i < trackedFlags.Length; i++)
        {
            if (trackedFlags[i].Equals(flag))
            {
                return flags[i];
            }
        }

        return false;
    }

    public ProgressionStats GetStats()
    {
        return stats;
    }

    /// <summary>
    /// If new items are added to the tracked flags, add them to the list of whether they've fired, and assume they have not.
    /// </summary>
    private void AddNewFlags()
    {
        if (flags.Count < trackedFlags.Count())
        {
            for (int i = flags.Count; i < trackedFlags.Count(); i++)
            {
                flags.Add(false);
            }
        }
    }

    #region Saving

    public class ProgressionSaveData
    {
        public List<bool> flags;
        public ProgressionStats stats;
    }

    public override void OnNewGame()
    {
        // create the list of flags, assuming none have fired yet
        flags = new List<bool>();
        foreach (string flag in trackedFlags)
        {
            flags.Add(false);
        }

        stats = new ProgressionStats()
        {
            victoryCount = 0,
            defeatCount = 0
        };
    }

    public override Saveable OnSave()
    {
        ProgressionSaveData saveData = new ProgressionSaveData()
        {
            flags = this.flags,
            stats = this.stats
        };

        string data = JsonUtility.ToJson(saveData);
        string identifier = $"{typeof(ProgressionSystem)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        // load saved data
        ProgressionSaveData saveData = JsonUtility.FromJson<ProgressionSaveData>(saveable.data);
        flags = saveData.flags;
        stats = saveData.stats;

        // check if new flags have been added
        AddNewFlags();
    }

    #endregion
}
