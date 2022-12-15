using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Represents generic actor in game.
    /// All heroes and enemies inherit from this class.
    /// Contains generic actor stats shared amongst heroes and enemies
    /// along with properties to access / modify these values as needed
    /// Performs attack and defense.
    /// </summary>
    public class Actor
    {
        private int defense;
        private int hitPoints;
        private int intelligence;
        private int magicDefense;
        private string name;
        private int speed;
        private string spriteName;
        private int strength;
        private bool isDefending;
        private int tagNumber;
        private Image image;

        public Actor() { }

        /// <summary>
        /// Actor constructore. Sets all actor stats to specified values.
        /// </summary>
        /// <param name="defense"></param>
        /// <param name="hitPoints"></param>
        /// <param name="intelligence"></param>
        /// <param name="magicDefense"></param>
        /// <param name="name"></param>
        /// <param name="speed"></param>
        /// <param name="spriteName"></param>
        /// <param name="strength"></param>
        /// <param name="isDefending"></param>
        public Actor(int defense, int hitPoints, int intelligence, int magicDefense, string name, int speed, string spriteName, int strength, bool isDefending)
        {
            this.Defense = defense;
            this.HitPoints = hitPoints;
            this.Intelligence = intelligence;
            this.MagicDefense = magicDefense;
            this.Name = name;
            this.Speed = speed;
            this.SpriteName = spriteName;
            this.Strength = strength;
            this.IsDefending = isDefending;
        }

        public bool IsDefending { get => isDefending; set => isDefending = value; }
        public int Defense { get => defense; set => defense = value; }
        public int HitPoints { get => hitPoints; set => hitPoints = value; }
        public int Intelligence { get => intelligence; set => intelligence = value; }
        public int MagicDefense { get => magicDefense; set => magicDefense = value; }
        public string Name { get => name; set => name = value; }
        public int Speed { get => speed; set => speed = value; }
        public string SpriteName { get => spriteName; set => spriteName = value; }
        public int Strength { get => strength; set => strength = value; }
        public int TagNumber { get => tagNumber; set => tagNumber = value; }
        public Image Image { get => image; set => image = value; }

        /// <summary>
        /// Performs attack on the specified target
        /// </summary>
        /// <param name="target">The target we are attacking</param>
        public void Attack(Actor target)
        {
            // calculate damage and deduct proper attribute
            int damage = (int)(this.Strength);
            if (target.IsDefending)
            {
                damage = (int)(damage - 1);
            }
            target.HitPoints = target.HitPoints - damage;
        }

        /// <summary>
        /// Toggles defnese status to true
        /// </summary>
        public void Defend()
        {
            this.IsDefending = true;
        }
    }
}