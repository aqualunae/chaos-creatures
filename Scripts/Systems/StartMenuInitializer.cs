using Unity.VisualScripting;
using UnityEngine;

public class StartMenuInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    private void Awake()
    {
        if (playerRef.Value != null)
        {
            playerRef.Value.gameObject.SetActive(false);
        }
    }
}