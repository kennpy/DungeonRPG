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
            this.defense = defense;
            this.hitPoints = hitPoints;
            this.intelligence = intelligence;
            this.magicDefense = magicDefense;
            this.name = name;
            this.speed = speed;
            this.spriteName = spriteName;
            this.strength = strength;
            this.isDefending = isDefending;
        }

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