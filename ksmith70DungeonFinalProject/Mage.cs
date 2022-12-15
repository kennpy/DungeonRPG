using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Mage : Hero
    {
/*        Image image = Properties.Resources.Mage;

        public Image Image { get => image; set => image = value; }*/

        public Mage()
        {
            this.Image = Properties.Resources.Mage;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Cool Mage";
            Speed = 8;
            SpriteName = "Mage";
            Strength = 2;
            IsDefending = false;
        }
    }
}