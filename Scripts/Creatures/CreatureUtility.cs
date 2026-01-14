using UnityEngine;

public static class CreatureUtility
{
    /// <summary>
    /// Bias the odds in favor of one or two indices. If the indices match, the effect is increased.
    /// </summary>
    public static int[] AdjustOdds(int[] defaultOdds, int firstIndex, int secondIndex = -1)
    {
        int[] adjustedOdds = new int[defaultOdds.Length];
        for (int i = 0; i < defaultOdds.Length; i++)
        {
            if (i == firstIndex && i == secondIndex)
            {
                adjustedOdds[i] = defaultOdds[i] * 5;
            }
            else if (i == firstIndex || i == secondIndex)
            {
                adjustedOdds[i] = defaultOdds[i] * 2;
            }
            else
            {
                adjustedOdds[i] = (int)(defaultOdds[i] * 0.6);
            }
        }
        return adjustedOdds;
    }
}
