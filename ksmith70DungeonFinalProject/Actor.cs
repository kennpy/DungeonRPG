using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
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


        public Actor() { }

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

        public void Attack(Actor target)
        {
            throw new System.NotImplementedException();
        }

        public void Defend()
        {
            throw new System.NotImplementedException();
        }
    }
}