using UnityEngine;

public class CharacterEncounter : MonoBehaviour
{
    [SerializeField]
    private GameObjectVariable opponentRef;

    [SerializeField]
    private CombatWindow combatWindow;

    public void StartCombat()
    {
        if (opponentRef.Value.TryGetComponent(out Party party))
        {
            combatWindow.gameObject.SetActive(true);
            combatWindow.Initialize(party);
        }
    }
}
