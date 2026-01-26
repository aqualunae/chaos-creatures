namespace Assets.Scripts.Creatures.Combat
{
    // aspect advantages described in Notes/aspects.json
    public enum Aspect
    {
        standard,
        earth,
        air,
        water,
        fire,
        plant,
        angel,
        demon
    }

    /// <summary>
    /// Used for combat
    /// </summary>
    [System.Serializable]
    public class Stats
    {
        public float hp;
        public float attack;
        public float defense;
        public float speed;
        public float critical;
        public Aspect aspect;
        public int currentHP;
        public int exp;
    }
}