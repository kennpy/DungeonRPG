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

        }
    }
}