using UnityEngine;

[RequireComponent(typeof(Party))]
public class CreatureStorage : MonoBehaviour
{
    [SerializeField, Tooltip("Reference that the storage window will check when opening.")]
    private GameObjectVariable storageRef;

    [SerializeField, Tooltip("The storage window itself.")]
    private CreatureStorageWindow storageWindow;

    /// <summary>
    /// Set this object as the active storage object and open the storage window.
    /// </summary>
    public void OpenStorage()
    {
        storageRef.Value = gameObject;
        storageWindow.gameObject.SetActive(true);
    }
}
