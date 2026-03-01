using UnityEngine;
using UnityEngine.Events;

public class ProgressionChecker : MonoBehaviour
{
    [System.Serializable]
    public class Check
    {
        public string flag;
        public UnityEvent result;
    }

    [SerializeField]
    private Check[] checks;

    [SerializeField]
    private GameObjectVariable progressionRef;

    private void Start()
    {
        CheckFlags();
    }

    private void CheckFlags()
    {
        ProgressionSystem progression = progressionRef.Value.GetComponent<ProgressionSystem>();
        foreach(Check check in checks)
        {
            if (progression.CheckFlag(check.flag))
            {
                check.result.Invoke();
            }
        }
    }
}
