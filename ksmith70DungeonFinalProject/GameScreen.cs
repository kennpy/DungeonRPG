﻿using System;
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
        private int currentAttackerId;

        public event EventHandler<TurnReadyEventArgs> TurnReady;

        List<PictureBox> heroPbs = new List<PictureBox>();
        List<PictureBox> enemyPbs = new List<PictureBox>();

        Bitmap[] heroSprites = new Bitmap[3];
        Bitmap[] enemySprites = new Bitmap[3];

        ProgressBar[] heroHealthBars = new ProgressBar[3];
        ProgressBar[] enemyHealthBars = new ProgressBar[3];

        Random random = new Random();

        public GameScreen()
        {
            InitializeComponent();
            PrepBoard();
            SubscribeFormHandlers();
            InstantiateProgressBars();

        }

        private void InstantiateProgressBars()
        {
            // add hero health bars
            heroHealthBars[0] = progressBar1;
            heroHealthBars[1] = progressBar2;
            heroHealthBars[2] = progressBar3;

            // add enemy health bars
            enemyHealthBars[0] = progressBar4;
            enemyHealthBars[1] = progressBar5;
            enemyHealthBars[2] = progressBar6;
        }

        private void SubscribeFormHandlers()
        {
            /*attackBtn.Enabled = true;
            defendBtn.Enabled = true;
            defendBtn.Enabled = true;*/

            attackBtn.Click += ActionButtonClick_Handler;
            defendBtn.Click += ActionButtonClick_Handler;
            specialBtn.Click += ActionButtonClick_Handler;

            /*enemyPb1.Click += EnemyPbClicked_Handler;
            enemyPb2.Click += EnemyPbClicked_Handler;
            enemyPb3.Click += EnemyPbClicked_Handler;*/
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
            heroPbs.Add(heroPb2);
            heroPbs.Add(heroPb3);

            enemyPbs.Add(enemyPb1);
            enemyPbs.Add(enemyPb2);
            enemyPbs.Add(enemyPb3);
        }

        // update picture box and health bar for new enctouner
        public void OnNewEncounter_Handler(object sender, NewEncounterEventArgs e)
        {
            Bitmap a; // bitmap stores pixel data 
            battleLog.AppendText("\r\nYou have been ambushed by some new enemies !");

            // flag the player turn ?? w/ background color change

            // delete all enemyPbs (outdate) and replace
            // unsibscribe enemyPb click handlers and re-enable for ones with enemies

            foreach (var enemyBox in enemyPbs)
            {
                // enemyBox.BackgroundImage = Image. ?? 
                //enemyBox.Visible = false;
                enemyBox.Click -= EnemyPbClicked_Handler;

            }


            for (int position = 0; position < heroPbs.Count; position++)
            {
                // ... 
                heroPbs[position].BackgroundImage = e.HeroSprites[position];
                heroHealthBars[position].Value = e.HeroHealth[position] * 10;
                heroPbs[position].Update();
                heroHealthBars[position].Update();
            }
            // same with enemy spries for statement
            // disbale their boxes until user selects an attack (happens in attackBtn handler)
            for (int position = 0; position < e.EnemySprites.Count; position++)
            {
                // ... 
                enemyPbs[position].BackgroundImage = e.EnemySprites[position];
                enemyHealthBars[position].Value = e.EnemyHealth[position] * 10;
                enemyPbs[position].Update();
                enemyHealthBars[position].Update();
                enemyPbs[position].Click += EnemyPbClicked_Handler;
                enemyPbs[position].Enabled = false;

            }
        }

        public  void OnPlayerChoice_Handler(object sener, PlayerChoiceEventArgs e)
        {
            // enable action buttons and disable enemies
            attackBtn.Enabled = true;
            defendBtn.Enabled = true;
            specialBtn.Enabled = true;

            // already disabled upon enemyPbClick so dont need to re-disable (i think)
           /* foreach(var enemyBox in enemyPbs)
            {
                enemyBox.Enabled = false;
            }*/

        }

        public void OnUpdate_Handler(object sender, UpdateEventArgs e)
        {

            // check if its a hero or an enemy
            // update the correct one

            currentAttackerId = e.TurnTag;

            if (e.TurnTag != -1)
            {

                if (!e.DefendWasChosen)
                {
                    battleLog.AppendText("\r\n" + e.AttackerName + " is attacking !");
                    battleLog.AppendText("\r\n" + e.TargetName + " was attacked !");
                    int newHealth = e.Health * 10;
                    if (e.TargetIsHero)
                    {
                        // if the target is still alive update bar else make them invisible
                        if (newHealth > 0)
                        {
                            // heroPbs[e.TurnTag - 1].BackColor = Color.Yellow;
                            heroHealthBars[e.TurnTag - 1].Value = newHealth; // HARD CODED HARD CODED
                            heroPbs[e.TurnTag - 1].Update();
                            heroHealthBars[e.TurnTag - 1].Update(); // HARD CODED HARD CODED


                            // eable enemy buttons so user can select them
                            // this only works becase turn has depth of one
                            // we'd have to pass whether the next turn is a hero's one
                            // or not within the updateEventArgs
                            /*  attackBtn.Enabled = true;
                              defendBtn.Enabled = true;
                              specialBtn.Enabled = true;*/

                            // only working with one enemy box for now
                            /* enemyPb1.Enabled = true;
                             enemyPb2.Enabled = true;
                             enemyPb3.Enabled = true;*/

                        }
                        else
                        {
                            battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                            heroPbs[e.TurnTag - 1].Visible = false;
                            heroHealthBars[e.TurnTag - 1].Value = 0;
                            progressBar1.Visible = false;
                        }
                    }
                    else
                    {
                        if (newHealth > 0)
                        {
                            // enemyPbs[e.TurnTag - 1].BackColor = Color.Yellow;
                            enemyHealthBars[e.TurnTag - 1].Value = newHealth; // HARD CODED HARD CODED (progress bar selection)
                            enemyHealthBars[e.TurnTag - 1].Update(); // HARD CODED HARD CODED (progress bar selection)
                            enemyPbs[e.TurnTag - 1].Update(); // DONT NEED ?? dont need ? its not updating anything 
                        }
                        else
                        {
                            battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                            enemyPbs[e.TurnTag - 1].Visible = false;
                            enemyHealthBars[e.TurnTag - 1].Value = 0;
                            progressBar2.Visible = false;
                        }
                    }
                }
                else
                {
                    // if the hero has defended update the battle log ?? 
                    if (e.TargetIsHero)
                    {
                        battleLog.AppendText("\r\n" + e.AttackerName + " is defending !");
                        battleLog.AppendText("\r\n" + e.TargetName + " defended !");
                    }

                }
            }
            else // the hero goes first (we change later to select first hero / enemy (based on turn tag -- )
            {
                heroPbs[0].BackColor = Color.Yellow;
                heroPbs[0].Update();
            }
           
        }

        public void BeatEncounter_Handler(object sender, EventArgs e)
        {
            battleLog.AppendText("\r\nYou beat the encounter ! Great job !");
        }
        public void LostGame_Handler(object sender, EventArgs e)
        {
            battleLog.AppendText("\r\nGame lost :(");
            MessageBox.Show("You lost the game ! Good try !");
            // disable all buttons
            attackBtn.Enabled = false;
            defendBtn.Enabled = false;
            specialBtn.Enabled = false;

            enemyPb1.Enabled = false;
            enemyPb2.Enabled = false;
            enemyPb3.Enabled = false;
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
            // enable all enemy buttons so user can select enemy if defend was not selected
            if (queuedAction != "Defend")
            {
                foreach (var enemyBox in enemyPbs)
                {
                    enemyBox.Enabled = true;
                }
            }
            // else re-enable attack so we can attack again
            else
            {
                attackBtn.Enabled = true;
                defendBtn.Enabled = true;
                specialBtn.Enabled = true;
            }
        }
        private void EnemyPbClicked_Handler(object sender, EventArgs e)
        {
            int targetTag = int.Parse((((PictureBox)sender).Tag).ToString());
            TurnReadyEventArgs args = new TurnReadyEventArgs();
            args.Attack = queuedAction;
            args.Enemy = targetTag;

            OnTurnReady(this, args);
            enemyPb1.Enabled = false;
            enemyPb1.Enabled = false;
            enemyPb1.Enabled = false;

            attackBtn.Enabled = true;
            defendBtn.Enabled = true;
            specialBtn.Enabled = true;

        }

        protected virtual void OnTurnReady(object sender, TurnReadyEventArgs e)
        {
            TurnReady.Invoke(this, e);

        }
    }
}
