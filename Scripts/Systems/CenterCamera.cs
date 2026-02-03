using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    private void Update()
    {
        transform.position = new Vector3(playerRef.Value.transform.position.x, playerRef.Value.transform.position.y, transform.position.z);
    }
}
