using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event that can be assigned through the Unity UI.
/// </summary>
/// <typeparam name="T">Event parameter type</typeparam>
public class ScriptableEvent<T> : ScriptableObject
{
    private UnityEvent<T> unityEvent;

    public void AddListener(UnityAction<T> unityAction)
    {
        this.unityEvent.AddListener(unityAction);
    }

    public void RemoveListener(UnityAction<T> unityAction)
    {
        this.unityEvent.RemoveListener(unityAction);
    }

    public void Invoke(T parameter)
    {
        unityEvent.Invoke(parameter);
    }
}
