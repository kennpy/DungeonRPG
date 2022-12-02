using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    public partial class GameScreen : Form
    {

        private string queuedAction;

        public event EventHandler<TurnReadyEventArgs> TurnReady;

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

            attackBtn.Click += ActionButtonClick_Handler;
            defendBtn.Click += ActionButtonClick_Handler;
            specialBtn.Click += ActionButtonClick_Handler;

            enemyPb1.Click += EnemyPbClicked_Handler;
            // enemyPb2.Click += EnemyPbClicked_Handler;
            // enemyPb3.Click += EnemyPbClicked_Handler;
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
            int tagNum = int.Parse((((PictureBox)sender).Tag).ToString());

        }

        private void defendBtn_Click(object sender, EventArgs e)
        {
        }

        private void PrepBoard()
        {
            heroPbs.Add(heroPb1);
            //heroPbs.Add(heroPb2);
            //heroPbs.Add(heroPb3);
            enemyPbs.Add(enemyPb1);
            //enemyPbs.Add(enemyPb2);
            //enemyPbs.Add(enemyPb3);
        }

        public void OnUpdate_Handler(object sender, UpdateEventArgs e)
        {
            // check if its a hero or an enemy
            // update the correct one


            if (e.TurnTag != -1)
            {

                if (!e.DefendWasChosen)
                {
                    battleLog.AppendText("\r\n" + e.TargetName + " was attacked !");



                    int newHealth = e.Health * 10;
                    if (e.TargetIsHero)
                    {
                        // if the target is still alive update bar else make them invisible
                        if (newHealth > 0)
                        {
                            heroPbs[e.TurnTag - 1].BackColor = Color.Yellow;
                            progressBar1.Value = newHealth; // HARD CODED HARD CODED
                            heroPbs[e.TurnTag - 1].Update();

                            // enable action buttons so user can select actions in next turn
                            /*attackBtn.Enabled = false;
                            defendBtn.Enabled = false;
                            specialBtn.Enabled = false;*/

                            // eable enemy buttons so user can select them
                            // this only works becase turn has depth of one
                            // we'd have to pass whether the next turn is a hero's one
                            // or not within the updateEventArgs
                            enemyPb1.Enabled = true;
                            attackBtn.Enabled = true;
                            defendBtn.Enabled = true;
                            specialBtn.Enabled = true;

                            // only working with one enemy box for now
                            /*enemyPb2.Enabled = true;
                            enemyPb3.Enabled = true;*/

                        }
                        else
                        {
                            battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                            heroPbs[e.TurnTag - 1].Visible = false;
                            progressBar1.Visible = false;
                        }
                    }
                    else
                    {
                        if (newHealth > 0)
                        {
                            // enemyPbs[e.TurnTag - 1].BackColor = Color.Yellow;
                            progressBar2.Value = e.Health * 10; // HARD CODED HARD CODED (progress bar selection)
                            enemyPbs[e.TurnTag - 1].Update();
                        }
                        else
                        {
                            battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                            enemyPbs[e.TurnTag - 1].Visible = false;
                            progressBar2.Visible = false;
                        }
                    }
                }
                else
                {
                    // if the enemy has defended update the battle log
                    if (e.TargetIsHero)
                    {
                        battleLog.AppendText("\r\n" + e.TargetName + " defended !");
                    }

                }
            }
            else // the hero goes first (we change later)
            {
                heroPbs[0].BackColor = Color.Yellow;
                heroPbs[0].Update();
            }
           
        }

        public void ActionButtonClick_Handler(object sender, EventArgs e)
        {
            string action = ((Button)sender).Tag.ToString();
            switch (action)
            {
                case "Attack":
                    battleLog.AppendText("\r\nAttack has been chosen !");
                    queuedAction = "Attack";
                    
                    attackBtn.Enabled = false;
                    defendBtn.Enabled = false;
                    specialBtn.Enabled = false;

                    foreach(var enemy in enemyPbs)
                    {
                        enemy.BackColor = Color.Red;
                    }

                    break;
                case "Defend":
                    battleLog.AppendText("\r\nDefend has been chosen !");
                    queuedAction = "Defend";

                    attackBtn.Enabled = false;
                    defendBtn.Enabled = false;
                    specialBtn.Enabled = false;

                    foreach (var enemy in enemyPbs)
                    {
                        enemy.BackColor = Color.Red;
                    }

                    // throw the event since we don't care about selecing an enemy (defend selects itself)

                    TurnReadyEventArgs args = new TurnReadyEventArgs();
                    args.Attack = queuedAction;
                    args.Enemy = 1; // HARD CODED HARD CODED HARD CODED HARD CODED HARD CODED 

                    OnTurnReady(this, args);

                    break;
                case "Special":

                    queuedAction = "Special";

                    attackBtn.Enabled = false;
                    defendBtn.Enabled = false;
                    specialBtn.Enabled = false;

                    foreach (var enemy in enemyPbs)
                    {
                        enemy.BackColor = Color.Red;
                    }
                    battleLog.AppendText("\r\nSpecial has been chosen !");
                    break;

            }
            // enable all enemy buttons so user can select enemy
            enemyPb1.Enabled = true;
            // enemyPb2.Enabled = true;
            // enemyPb3.Enabled = true;
        }
        private void EnemyPbClicked_Handler(object sender, EventArgs e)
        {
            int targetTag = int.Parse((((PictureBox)sender).Tag).ToString());
            TurnReadyEventArgs args = new TurnReadyEventArgs();
            args.Attack = queuedAction;
            args.Enemy = targetTag;
            OnTurnReady(this, args);
            enemyPb1.Enabled = false;

        }

        protected virtual void OnTurnReady(object sender, TurnReadyEventArgs e)
        {
            TurnReady.Invoke(this, e);

        }
    }
}
