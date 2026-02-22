using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ItemStorage : MonoBehaviour
{
    [SerializeField, Tooltip("Reference that the storage window will check when opening.")]
    private GameObjectVariable storageRef;

    [SerializeField, Tooltip("The storage window itself.")]
    private ItemStorageWindow itemStorageWindow;

    /// <summary>
    /// Set this object as the active storage object and open the storage window.
    /// </summary>
    public void OpenStorage()
    {
        storageRef.Value = gameObject;
        itemStorageWindow.gameObject.SetActive(true);
    }
}
