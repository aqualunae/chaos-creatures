using UnityEngine;

[RequireComponent(typeof(Party))]
public class CreatureStorage : MonoBehaviour
{
    [SerializeField, Tooltip("The storage window itself.")]
    private CreatureStorageWindow storageWindow;

    /// <summary>
    /// Set this object as the active storage object and open the storage window.
    /// </summary>
    public void OpenStorage()
    {
        storageWindow.gameObject.SetActive(true);
    }
}
