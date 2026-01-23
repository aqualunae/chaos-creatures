using UnityEngine;

[RequireComponent(typeof(CreatureEgg))]
public class SetEgg : MonoBehaviour
{
    [SerializeField]
    private Variable<CreatureEgg> egg;

    private void Awake()
    {
        egg.Value = GetComponent<CreatureEgg>();
    }
}
