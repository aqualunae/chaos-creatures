using UnityEngine;

public class HatchEgg : MonoBehaviour
{
    [SerializeField]
    private EggVariable eggVariable;

    public void Hatch()
    {
        if (eggVariable.Value != null)
        {
            eggVariable.Value.Hatch();
        }
    }
}
