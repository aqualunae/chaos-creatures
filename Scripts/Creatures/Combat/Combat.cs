namespace Assets.Scripts.Creatures.Combat
{
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

    [System.Serializable]
    public class Stats
    {
        public float hp;
        public float attack;
        public float defense;
        public float speed;
        public float critical;
        public Aspect aspect;
    }
}