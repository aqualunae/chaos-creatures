using UnityEngine;

public class TimeSystem : SaveableBehaviour
{
    [SerializeField]
    private IntEvent ticker;

    private float playTime;
    private int seconds;

    private void Update()
    {
        playTime += Time.deltaTime;
        if ((int)playTime > seconds)
        {
            seconds = (int)playTime;
            ticker.Invoke(seconds);
        }
    }

    #region saving

    public class TimeSaveData
    {
        public float playTime;
    }

    public override Saveable OnSave()
    {
        TimeSaveData saveData = new TimeSaveData()
        {
            playTime = this.playTime
        };

        string data = JsonUtility.ToJson(saveData);
        string identifier = $"{typeof(TimeSystem)}_{id}";

        Saveable saveable = new Saveable()
        {
            id = identifier,
            data = data
        };

        return saveable;
    }

    public override void OnLoad(Saveable saveable)
    {
        TimeSaveData saveData = JsonUtility.FromJson<TimeSaveData>(saveable.data);
        playTime = saveData.playTime;
    }

    public override void OnNewGame()
    {
        playTime = 0;
    }

    #endregion
}
