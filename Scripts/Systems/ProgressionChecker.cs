using UnityEngine;
using UnityEngine.Events;

public class ProgressionChecker : MonoBehaviour
{
    [System.Serializable]
    public class Check
    {
        public string flag;
        public UnityEvent<int> result;
    }

    [SerializeField]
    private Check[] checks;

    [SerializeField]
    private GameObjectVariable progressionRef;

    [SerializeField, Tooltip("Event that is fired when the player makes progress in the game.")]
    private StringEvent progressionTrigger;

    private void Start()
    {
        CheckFlags();
        progressionTrigger.AddListener(CheckFlags);
    }

    private void CheckFlags()
    {
        ProgressionSystem progression = progressionRef.Value.GetComponent<ProgressionSystem>();
        for (int i = 0; i < checks.Length; i++)
        {
            if (progression.CheckFlag(checks[i].flag))
            {
                checks[i].result.Invoke(i);
            }
        }
    }

    private void CheckFlags(string trigger)
    {
        for (int i = 0; i < checks.Length; i++)
        {
            if (checks[i].flag.Equals(trigger))
            {
                checks[i].result.Invoke(i);
            }
        }
    }
}
