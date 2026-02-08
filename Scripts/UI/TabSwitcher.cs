using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tabs;

    [SerializeField]
    private Button[] buttons;

    /// <summary>
    /// Show the selected tab and hide all others.
    /// </summary>
    public void SwitchTab(GameObject selected)
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(tab == selected);
        }
    }

    /// <summary>
    /// To show that you're already on this tab, disable clicking on it again.
    /// </summary>
    public void DisableButton(Button selected)
    {
        foreach(Button button in buttons)
        {
            button.interactable = button != selected;
        }
    }

    /// <summary>
    /// Switch to a tab and disable its corresponding button at the same time
    /// </summary>
    /// <param name="index">Index of the tab</param>
    public void AutoSwitch(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
            buttons[i].interactable = i != index;
        }
    }
}
