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

    public class Stats
    {
        public int hp;
        public int attack;
        public int defense;
        public int speed;
        public int critical;
        public Aspect aspect;
    }
}