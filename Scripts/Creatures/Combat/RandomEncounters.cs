using Assets.Scripts.Creatures;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Tilemap))]
public class RandomEncounters : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    private RandomCreature[] possibleCreatures;

    [SerializeField]
    private CombatWindow combatWindow;

    [SerializeField, Tooltip("One in how many chance that combat will start?")]
    private int combatChance = 3;

    [SerializeField, Tooltip("How many steps between assessing combat chance?")]
    private int frequency = 3;

    [SerializeField]
    private Vector3Event movementEvent;

    private int movementElapsed;
    private Tilemap tilemap;

    private void Awake()
    {
        // get the tilemap and listen for player movement
        tilemap = GetComponent<Tilemap>();
        movementEvent.AddListener(OnMove);
    }

    private void OnDisable()
    {
        movementEvent.RemoveListener(OnMove);
    }

    private void OnMove(Vector3 position)
    {
        // when the player moves, check whether they're on the tilemap
        Vector3Int cellposition = tilemap.WorldToCell(position);
        if (tilemap.HasTile(cellposition))
        {
            // if they are, there's a chance of starting combat
            movementElapsed++;
            if (movementElapsed > frequency)
            {
                StartCombat();
            }
        }
    }

    /// <summary>
    /// Generate a random creature and open the combat window
    /// </summary>
    private void StartCombat()
    {
        int chance = UnityEngine.Random.Range(0, combatChance);
        if (chance == 0)
        {
            int index = UnityEngine.Random.Range(0, possibleCreatures.Length);
            SaveableCreature creature = possibleCreatures[index].GetRandomCreature();
            combatWindow.gameObject.SetActive(true);
            combatWindow.Initialize(creature);

            // set movement since last combat to 0
            movementElapsed = 0;
        }
    }
}
