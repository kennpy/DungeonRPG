using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Dragon : Enemy
    {
 /*       Image image = Properties.Resources.Dragon; 

        public Image Image { get => image; set => image = value; }*/
        public Dragon()
        {
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Sick Sinner";
            Speed = 9;
            SpriteName = "Dragon";
            Strength = 2;
            IsDefending = false;

            this.Image = Properties.Resources.Dragon;

        }

    }
}