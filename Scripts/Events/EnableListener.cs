using UnityEngine;
using UnityEngine.Events;

public class EnableListener : MonoBehaviour
{
    [SerializeField]
    private UnityEvent uEvent;

    private void OnEnable()
    {
        uEvent.Invoke();
    }
}
