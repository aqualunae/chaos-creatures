using UnityEngine;

/// <summary>
/// Used by the Generator.
/// </summary>
public class ResetEgg : MonoBehaviour
{
    [SerializeField]
    private CreatureVariable creatureVariable;

    [SerializeField]
    private CreatureEgg egg;

    public void Reset()
    {
        if (creatureVariable.Value != null)
        {
           creatureVariable.Value.gameObject.SetActive(false); 
        }
        CreatureEgg instantiatedEgg = Instantiate(egg);
        instantiatedEgg.InitializeDefault();
        instantiatedEgg.transform.localScale = Vector2.one;
        instantiatedEgg.transform.position = new Vector2(0.48f, -0.32f);
    }
}
