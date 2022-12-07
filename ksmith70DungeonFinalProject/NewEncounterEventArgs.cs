using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ksmith70DungeonFinalProject
{
    public class NewEncounterEventArgs
    {

        Bitmap[] heroSprites = new Bitmap[3];
        Bitmap[] enemySprites = new Bitmap[3];

        public Bitmap[] HeroSprites { get => heroSprites; set => heroSprites = value; }
        public Bitmap[] EnemySprites { get => enemySprites; set => enemySprites = value; }
    }
}
