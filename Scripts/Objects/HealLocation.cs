using UnityEngine;

public class HealLocation : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable playerRef;

    public void HealParty()
    {
        playerRef.Value.GetComponent<Party>().HealAll();
    }
}
