using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    public partial class GameScreen : Form
    {
        public event EventHandler TurnReady;

        List<PictureBox> heroPbs = new List<PictureBox>();
        List<PictureBox> enemyPbs = new List<PictureBox>();

        public GameScreen()
        {
            InitializeComponent();
            PrepBoard();
            SubscribeFormHandlers();

        }

        private void SubscribeFormHandlers()
        {
            attackBtn.Enabled = true;
            defendBtn.Enabled = true;
            defendBtn.Enabled = true;
            button1.Enabled = true;

            button1.EnabledChanged += OnEnabledChanged;
            button1.StyleChanged += OnEnabledChanged;
            button1.Invalidated += OnEnabledChanged;
            button1.Validating += OnEnabledChanged;
            button1.Validated += OnEnabledChanged;

            attackBtn.Click += ActionButtonClick_Handler;
            defendBtn.Click += ActionButtonClick_Handler;
            specialBtn.Click += ActionButtonClick_Handler;
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            // battleLog.AppendText("disabled !");
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // get the tag so we know which was clicked
            int tagNum = (int)((PictureBox)sender).Tag;
            battleLog.AppendText("ONE CLICKED ");

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

        public void OnUpdate_Handler(object sender, UpdateEventArgs e)
        {
            // check if its a hero or an enemy
            // update the correct one
            // if isPlayerTurn set to false update enemy else player
            heroPbs[e.HeroTurnTag - 1].BackColor = Color.Yellow;
            heroPbs[e.HeroTurnTag - 1].Update();
            battleLog.Text = (e.HeroTurnTag + " is taking a turn !");
           
        }

        public void ActionButtonClick_Handler(object sender, EventArgs e)
        {
            string action = ((Button)sender).Tag.ToString();
            switch (action)
            {
                case "Attack":
                    battleLog.AppendText("\r\nAttack has been chosen !");
                    break;
                case "Defend":
                    battleLog.AppendText("\r\nDefend has been chosen !");
                    break;
                case "Special":
                    battleLog.AppendText("\r\nSpecial has been chosen !");
                    break;

            }
                    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            battleLog.AppendText("WORKS WORKS WORKS");
        }
    }
}
