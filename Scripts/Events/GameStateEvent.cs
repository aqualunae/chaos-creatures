using UnityEngine;

public enum GameState
{
    Overworld,
    PauzeMenu,
    CombatWindow,
    AdoptionWindow,
    DialogueWindow,
    StorageWindow
}

[System.Serializable, CreateAssetMenu(fileName = "Event ", menuName = "Events/Game State")]
public class GameStateEvent : ScriptableEvent<GameState>
{
    
}
