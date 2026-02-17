using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ItemStorage : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable storageRef;

    [SerializeField]
    private ItemStorageWindow itemStorageWindow;

    public void OpenStorage()
    {
        storageRef.Value = gameObject;
        itemStorageWindow.gameObject.SetActive(true);
    }
}
