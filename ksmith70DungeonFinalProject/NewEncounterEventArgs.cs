using System.Collections.Generic;
using System.Drawing;
using System;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Event args which tell GameScreen the details of the encounter
    /// including hero / enemy sprites and health status
    /// </summary>
    public class NewEncounterEventArgs : EventArgs
    {
        // list of sprite images
        List<Bitmap> heroSprites = new List<Bitmap>();
        List<Bitmap> enemySprites = new List<Bitmap>();

        // list of health status's
        List<int> heroHealth = new List<int>();
        List<int> enemyHealth = new List<int>();

        public List<Bitmap> HeroSprites { get => heroSprites; set => heroSprites = value; }
        public List<Bitmap> EnemySprites { get => enemySprites; set => enemySprites = value; }
        public List<int> HeroHealth { get => heroHealth; set => heroHealth = value; }
        public List<int> EnemyHealth { get => enemyHealth; set => enemyHealth = value; }
    }
}
