using UnityEngine;

public class LootableItem : SaveableBehaviour
{
    [SerializeField, Tooltip("Possible items that could be picked up here.")]
    private LootTable table;

    [SerializeField, Tooltip("Reference to the player.")]
    private GameObjectVariable playerRef;

    [SerializeField, Tooltip("World sprite that displays the lootable.")]
    private SpriteRenderer lootRenderer;

    [SerializeField, Tooltip("Event that fires the current play time in seconds.")]
    private IntEvent ticker;

    [SerializeField, Tooltip("After the loot is picked up, how many seconds until it should spawn a new loot item?")]
    private int interval;

    [SerializeField]
    private ColorListVariable colorList;

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
        lootRenderer.color = colorList.GetColor(loot.item.Color);
        available = true;
    }

    /// <summary>
    /// Add the item to the player's inventory.
    /// </summary>
    public void Pickup()
    {
        // if the item isn't available, correct its visibility and don't do anything else.
        if (!available)
        {
            lootRenderer.gameObject.SetActive(false);
            return;
        }

        // add item returns false if the player's inventory is full
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

        // if the time elapsed since the last pickup is greater than the interval
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
        lootRenderer.color = colorList.GetColor(loot.item.Color);
        lootRenderer.gameObject.SetActive(available);
    }

    public override void OnNewGame()
    {
        Initialize();
    }

    #endregion
}
