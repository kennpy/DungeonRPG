using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace ksmith70DungeonFinalProject
{
    public class Bandit : Enemy
    {
        /*Image image = Properties.Resources.Bandit; 

        public Image Image { get => image; set => image = value; }*/

        public Bandit()
        {
            this.Image = Properties.Resources.Bandit;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Jerry";
            Speed = 5;
            SpriteName = "Bandit";
            Strength = 2;
            IsDefending = false;
        }
    }
}