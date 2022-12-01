using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    public partial class GameScreen : Form
    {
        public event EventHandler TurnReady;

        List<PictureBox> heroPbs = new List<PictureBox>();
        List<PictureBox> enemyPbs new List<PictureBox>();

        public GameScreen()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // get the tag so we know which was clicked
            int tagNum = (int)((PictureBox)sender).Tag;

        }

        private void PrepBoard()
        {
            heroPbs.Add(heroPb1);
            heroPbs.Add(heroPb2);
            heroPbs.Add(heroPb3);
            enemyPbs.Add(enemyPb1);
            enemyPbs.Add(enemyPb2);
            enemyPbs.Add(enemyPb3);
        }
    }
}
