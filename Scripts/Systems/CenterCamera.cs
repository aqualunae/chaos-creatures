using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToCenter;

    private void Update()
    {
        transform.position = new Vector3(objectToCenter.transform.position.x, objectToCenter.transform.position.y, transform.position.z);
    }
}
