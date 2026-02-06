using UnityEngine;

public enum GameState
{
    Overworld,
    PauzeMenu,
    OtherMenu
}

[System.Serializable, CreateAssetMenu(fileName = "Event ", menuName = "Events/Game State")]
public class GameStateEvent : ScriptableEvent<GameState>
{
    
}
