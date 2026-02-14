using UnityEngine;
using UnityEngine.Events;

public class SelectionListener : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> selectionEvent;

    [SerializeField]
    private UnityEvent<int> activationEvent;

    public void OnSelect(int index)
    {
        selectionEvent.Invoke(index);
    }

    public void OnActivate(int index)
    {
        activationEvent.Invoke(index);
    }
}
