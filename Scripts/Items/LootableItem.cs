using UnityEngine;

public class LootableItem : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private int amount;

    [SerializeField]
    private GameObjectVariable playerRef;

    public void Pickup()
    {
        if (playerRef.Value.GetComponent<Inventory>().AddItem(item, amount))
        {
            gameObject.SetActive(false);
        }
    }
}
