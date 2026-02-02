using UnityEngine;

public enum SaveState
{
    NewGame,
    Save,
    Load
}

[CreateAssetMenu(fileName = "Event ", menuName = "Events/Save")]
public class SaveEvent : ScriptableEvent<SaveState>
{
    
}
