using UnityEngine;
using UnityEngine.Events;

public class CancelListener : MonoBehaviour
{
    [SerializeField]
    private VoidEvent cancelEvent;

    [SerializeField]
    private UnityEvent OnCancel;

    private void CancelHandler()
    {
        OnCancel.Invoke();
    }

    private void OnEnable()
    {
        cancelEvent.AddListener(CancelHandler);
    }

    private void OnDisable()
    {
        cancelEvent.RemoveListener(CancelHandler);
    }
}
