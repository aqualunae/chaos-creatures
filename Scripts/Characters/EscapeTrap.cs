using UnityEngine;

public class EscapeTrap : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    public void Escape()
    {
        playerRef.Value.GetComponent<WarpSelf>().WarpToTarget();
    }
}
