using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayKeybind : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bindingField;

    [SerializeField]
    private string actionTitle;

    [SerializeField]
    private GameObjectVariable playerRef;

    private void OnEnable()
    {
        PlayerInput input = playerRef.Value.GetComponent<PlayerInput>();
        bindingField.text = input.currentActionMap.FindAction(actionTitle).GetBindingDisplayString();
    }
}
