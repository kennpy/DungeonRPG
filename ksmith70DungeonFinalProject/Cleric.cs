using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ksmith70DungeonFinalProject
{
    public class Cleric : Hero
    {
        /*Image image = Properties.Resources.Cleric;

        public Image Image { get => image; set => image = value; }*/
        public Cleric()
        {
            this.Image = Properties.Resources.Cleric;
            Defense = 8;
            HitPoints = 10;
            Intelligence = 3;
            MagicDefense = 2;
            Name = "Sad Nike Promotion";
            Speed = 5;
            SpriteName = "Cleric";
            Strength = 2;
            IsDefending = false;
        }
    }
}