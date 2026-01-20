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

    protected static readonly HashSet<SaveableBehaviour> instances = new HashSet<SaveableBehaviour>();
    public static HashSet<SaveableBehaviour> Instances
    {
        get => new HashSet<SaveableBehaviour>(instances);
    }

    [SerializeField]
    protected string id = Guid.NewGuid().ToString();

    public string ID
    {
        get => id;
    }

    public abstract void OnNewGame();

    public abstract Saveable OnSave();

    public abstract void OnLoad(Saveable saveable);
}
