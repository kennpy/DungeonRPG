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

        List<Bitmap> heroSprites = new List<Bitmap>();
        List<Bitmap> enemySprites = new List<Bitmap>();

        List<int> heroHealth = new List<int>();
        List<int> enemyHealth = new List<int>();

        // ADD SKILL POINTS ?? etc. ?? 
        // ADD SKILL POINTS ?? etc. ?? 

        public List<Bitmap> HeroSprites { get => heroSprites; set => heroSprites = value; }
        public List<Bitmap> EnemySprites { get => enemySprites; set => enemySprites = value; }
        public List<int> HeroHealth { get => heroHealth; set => heroHealth = value; }
        public List<int> EnemyHealth { get => enemyHealth; set => enemyHealth = value; }
    }
}
