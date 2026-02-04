using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SaveableBehaviour : MonoBehaviour
{
    /// <summary>
    /// ID format: class_guid | Data format: JSON
    /// </summary>
    [System.Serializable]
    public class Saveable
    {
        public string id;
        public string data;
    }

    /// <summary>
    /// Hashset of SaveableBehaviours. All SBs must add themselves to this list on Awake().
    /// </summary>
    protected static Dictionary<string, SaveableBehaviour> instances = new Dictionary<string, SaveableBehaviour>();

    /// <summary>
    /// Copy of the HashSet that cannot be changed. Save System iterates through it to retrieve and send out save data.
    /// </summary>
    public static HashSet<SaveableBehaviour> Instances
    {
        get => new HashSet<SaveableBehaviour>(instances.Values);
    }

    /// <summary>
    /// Add self to the save data, or update reference if already there.
    /// </summary>
    protected void Awake()
    {
        if (instances.ContainsKey(ID))
        {
            instances[ID] = this;
        }
        else
        {
            instances.Add(ID, this);
        }
    }

    [SerializeField, Tooltip("Used as a unique identifier for this object.")]
    protected string id = Guid.NewGuid().ToString();

    public string ID
    {
        get => id;
        set => id = value;
    }

    [SerializeField]
    protected SaveEvent saveEvent;

    /// <summary>
    /// How should the object initialize when it has no save data available?
    /// </summary>
    public abstract void OnNewGame();

    /// <summary>
    /// What data does the object need to save?
    /// </summary>
    public abstract Saveable OnSave();

    /// <summary>
    /// How should the object load in save data?
    /// </summary>
    public abstract void OnLoad(Saveable saveable);

    /// <summary>
    /// Can be called to update the save data.
    /// </summary>
    protected void TriggerSave()
    {
        saveEvent.Invoke(SaveState.Save);
    }

    /// <summary>
    /// Can be called to remove this from the save data.
    /// </summary>
    protected void RemoveSelf()
    {
        instances.Remove(ID);
    }
}
