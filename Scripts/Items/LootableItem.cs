using UnityEngine;

public class LootableItem : SaveableBehaviour
{
    [SerializeField]
    private LootTable table;

    [SerializeField]
    private GameObjectVariable playerRef;

    [SerializeField]
    private SpriteRenderer lootRenderer;

    [SerializeField]
    private IntEvent ticker;

    [SerializeField]
    private int interval;

    private Lootable loot;
    private bool available = true;
    private int lastInterval = 0;
    private bool needIntervalReset = false;

    // setting the active state of the sprite instead of the parent (this)
    // so that the ticker listener works while the item is not available

    private void Start()
    {
        ticker.AddListener(OnTick);
    }

    /// <summary>
    /// Select and render an item.
    /// </summary>
    private void Initialize()
    {
        lootRenderer.gameObject.SetActive(true);
        loot = table.GetLootable();
        lootRenderer.sprite = loot.item.Sprite;
        lootRenderer.color = loot.item.Color;
        available = true;
    }

    /// <summary>
    /// Add the item to the player's inventory.
    /// </summary>
    public void Pickup()
    {
        if (!available)
        {
            lootRenderer.gameObject.SetActive(false);
            return;
        }

        if (playerRef.Value.GetComponent<Inventory>().AddItem(loot.item, loot.amount))
        {
            lootRenderer.gameObject.SetActive(false);
            needIntervalReset = true;
            available = false;
        }
    }

    /// <summary>
    /// Evaluate whether to respawn the item
    /// </summary>
    public void OnTick(int seconds)
    {
        // if the item has just been looted
        if (needIntervalReset)
        {
            // set the interval timer to start now
            lastInterval = seconds;
            needIntervalReset = false;
            return;
        }

        // if the time elapsed since the last initialization is greater than the interval
        // and the item has been looted
        if (seconds - lastInterval > interval && !available)
        {
            // respawn the item
            Initialize();
        }
    }

    private void OnDisable()
    {
        // clean up listener
        ticker.RemoveListener(OnTick);
    }

    #region Saving

    public class LootSaveData
    {
        public int index;
        public bool available;
        public int lastInterval;
    }

    public override Saveable OnSave()
    {
        LootSaveData saveData = new LootSaveData()
        {
            index = loot.index,
            available = this.available,
            lastInterval = this.lastInterval
        };

        string data = JsonUtility.ToJson(saveData);
        string identifier = $"{typeof(LootableItem)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        // load data
        LootSaveData saveData = JsonUtility.FromJson<LootSaveData>(saveable.data);
        loot = table.GetLootable(saveData.index); 
        available = saveData.available;
        lastInterval = saveData.lastInterval;

        // render item
        lootRenderer.sprite = loot.item.Sprite;
        lootRenderer.color = loot.item.Color;
        lootRenderer.gameObject.SetActive(available);
    }

    public override void OnNewGame()
    {
        Initialize();
    }

    #endregion
}
