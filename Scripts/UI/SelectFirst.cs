using UnityEngine;
using UnityEngine.EventSystems;

public class SelectFirst : MonoBehaviour
{
    [SerializeField]
    private GameObject selectFirst;

    [SerializeField]
    private GameObjectVariable eventSystemRef;

    private void OnEnable()
    {
        if (eventSystemRef.Value != null)
        {
            eventSystemRef.Value.GetComponent<EventSystem>().firstSelectedGameObject = selectFirst;
        }
    }
}
