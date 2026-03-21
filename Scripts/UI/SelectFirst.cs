using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectFirst : MonoBehaviour
{
    [SerializeField]
    private Button selectFirst;

    private void OnEnable()
    {
        selectFirst.Select();
    }

    public void Select()
    {
        selectFirst.Select();
    }
}
