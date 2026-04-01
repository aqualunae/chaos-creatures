using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "World/Cutscene")]
public class Cutscene : ScriptableObject
{
    [SerializeField, Tooltip("Title may be displayed to the player")]
    private string title;

    [SerializeField, Tooltip("ID_keyframeIndex is used to label animations")]
    private string id;

    [System.Serializable]
    public class Keyframe
    {
        public string source;
        public string[] lines;
    }

    [SerializeField, Tooltip("Dialogue and movements")]
    private Keyframe[] keyframes;

    [SerializeField, Tooltip("Actors to control for the cutscene")]
    private GameObject[] actors;

    public string Title
    {
        get => title;
    }

    public string ID
    {
        get => id;
    }

    public Keyframe[] Keyframes
    {
        get => keyframes;
    }

    public GameObject[] Actors
    {
        get => actors;
    }
}
