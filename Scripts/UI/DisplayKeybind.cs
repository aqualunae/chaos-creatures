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
        if (input.actions.FindActionMap("Player").enabled)
        {
            bindingField.text = input.actions.FindActionMap("Player").FindAction(actionTitle).GetBindingDisplayString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
