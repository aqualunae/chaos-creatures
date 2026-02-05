using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "Event ", menuName = "Events/Void")]
public class VoidEvent : ScriptableObject
{
    private UnityEvent unityEvent = new UnityEvent();

    public void AddListener(UnityAction unityAction)
    {
        this.unityEvent.AddListener(unityAction);
    }

    public void RemoveListener(UnityAction unityAction)
    {
        this.unityEvent.RemoveListener(unityAction);
    }

    public void Invoke()
    {
        unityEvent.Invoke();
    }
}
