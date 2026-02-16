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
    [SerializeField, Tooltip("List of random creatures that can spawn.")]
    private RandomCreature[] possibleCreatures;

    [SerializeField, Tooltip("Combat window to open.")]
    private CombatWindow combatWindow;

    [SerializeField, Tooltip("One in how many chance that combat will start?")]
    private int combatChance = 3;

    [SerializeField, Tooltip("How many steps between assessing combat chance?")]
    private int frequency = 3;

    [SerializeField, Tooltip("Event that is fired when the player moves.")]
    private Vector3Event movementEvent;

    // number of times the player has moved over the selected tilemap since the last instance of combat
    private int movementElapsed;

    // the tilemap to which this script is attached
    private Tilemap tilemap;

    private void Awake()
    {
        // get the tilemap and listen for player movement
        tilemap = GetComponent<Tilemap>();
        movementEvent.AddListener(OnMove);
    }

    private void OnDisable()
    {
        // clean up listener
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
        // decide if combat is starting
        int chance = UnityEngine.Random.Range(0, combatChance);
        if (chance == 0)
        {
            // randomly select and generate a creature
            int index = UnityEngine.Random.Range(0, possibleCreatures.Length);
            SaveableCreature creature = possibleCreatures[index].GetRandomCreature();

            // activate the combat window and initialize it with the opponent
            combatWindow.gameObject.SetActive(true);
            combatWindow.Initialize(creature);

            // set movement since last combat to 0
            movementElapsed = 0;
        }
    }
}
