using System.Linq;
using UnityEngine;

/// <summary>
/// Source of truth for the list of possible species. Only create one.
/// </summary>
[CreateAssetMenu(fileName = "Species List", menuName = "Variables/Species List")]
public class SpeciesListVariable: Variable<CreatureSpecies[]>
{
    /// <summary>
    /// Get a species scriptable object by name. Species contain a lot of sprites and complex data, so they're saved by string.
    /// </summary>
    /// <param name="speciesTitle">String species name.</param>
    /// <returns>Scriptable object containing data about the species.</returns>
    public CreatureSpecies GetSpecies(string speciesTitle)
    {
        Debug.Log(speciesTitle);
        return Value.First(species => species.Species == speciesTitle);
    }
}
