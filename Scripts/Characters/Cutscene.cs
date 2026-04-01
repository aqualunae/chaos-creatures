using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "World/Cutscene")]
public class Cutscene : ScriptableObject
{
    // cutscene title (might be displayed to player)
    // cutscene id (used to label animations)
    // triggers that unlock the cutscene (string)
    // conditions that activate the cutscene (scene)
    // array of keyframes
    // each containing: one or more lines of dialogue, optional animation
    // objects to instantiate for animation
}
