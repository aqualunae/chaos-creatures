using UnityEngine;

[RequireComponent(typeof(ChaosCreature))]
public class SetCreature : MonoBehaviour
{
    [SerializeField]
    private Variable<ChaosCreature> creature;

    private void Awake()
    {
        creature.Value = GetComponent<ChaosCreature>();
    }
}
