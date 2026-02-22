using UnityEngine;

/// <summary>
/// Used to control the pauze state of the game
/// </summary>
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
