using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
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
            // THIS WAS NOT ENABLED ORIGINALLY
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

            // reset enemy handlers, picture boxes, and progress bars
            RemoveEnemyClickHandlers();
            HideEnemyPictureBoxes();
            HideEnemyProgressBars();

            for (int position = 0; position < e.HeroSprites.Count; position++)
            {
                SetInitialHeroState(e, position);
            }
            // same with enemy spries for statement
            // disbale their boxes until user selects an attack (happens in attackBtn handler)
            for (int position = 0; position < e.EnemySprites.Count; position++)
            {
                SetInitialEnemyState(e, position);
            }
        }

        private void RemoveEnemyClickHandlers()
        {
            foreach (var enemyBox in enemyPbs)
            {
                enemyBox.Click -= EnemyPbClicked_Handler;
            }
        }

        private void SetInitialEnemyState(NewEncounterEventArgs e, int position)
        {
            enemyPbs[position].Visible = true;
            enemyHealthBars[position].Visible = true;
            enemyPbs[position].BackgroundImage = e.EnemySprites[position];
            enemyHealthBars[position].Value = e.EnemyHealth[position] * 10;
            enemyPbs[position].Click += EnemyPbClicked_Handler;
            enemyPbs[position].Enabled = false;
            enemyPbs[position].Update();
            enemyHealthBars[position].Update();

        }

        private void HideEnemyProgressBars()
        {
            foreach (var bar in enemyHealthBars)
            {
                bar.Visible = false;
            }
        }
        private void HideEnemyPictureBoxes()
        {
            foreach(var box in enemyPbs)
            {
                box.Visible = false;
            }
        }
       

        
        private void SetInitialHeroState(NewEncounterEventArgs e, int position)
        {
            if (e.HeroHealth[position] > 0)
            {
                // ... 
                heroPbs[position].BackgroundImage = e.HeroSprites[position];
                heroHealthBars[position].Value = e.HeroHealth[position] * 10;
                heroPbs[position].Update();
                heroHealthBars[position].Update();
            }
        }

        public  void OnPlayerChoice_Handler(object sener, CurrentAttackerEventArgs e)
        {
            // enable action buttons and disable enemies
            ToggleActionBtnEnable(true);

            ResetPictureBoxBackgrounds();

            if (e.AttackerIsHero)
            {
                heroPbs[e.PlayerTag - 1].BackColor = Color.Yellow;
            }
            else
            {
                enemyPbs[e.PlayerTag - 1].BackColor = Color.Yellow;
            }
        }

        public void OnUpdate_Handler(object sender, UpdateEventArgs e)
        {

            // check if its a hero or an enemy
            // update the correct one

            currentAttackerId = e.TurnTag;

            if (e.TurnTag != -1)
            {
                // UpdateAttackerBackground(e); 
                if (!e.DefendWasChosen)
                {
                    ShowAttack(e);
                }
                else
                {
                    ShowDefense(e);
                }
            }
            else // the hero goes first (we change later to select first hero / enemy (based on turn tag -- )
            {
                heroPbs[0].BackColor = Color.Yellow;
                heroPbs[0].Update();
            }
           
        }

        private void ShowDefense(UpdateEventArgs e)
        {
            // if the hero has defended update the battle log 
            if (e.TargetIsHero)
            {
                battleLog.AppendText("\r\n" + e.AttackerName + " is defending !");
                battleLog.AppendText("\r\n" + e.TargetName + " defended !");
            }
        }

        private void ShowAttack(UpdateEventArgs e)
        {
            battleLog.AppendText("\r\n" + e.AttackerName + " is attacking !");
            battleLog.AppendText("\r\n" + e.TargetName + " was attacked !");

            int newHealth = CalculateHealth(e);
            if (e.TargetIsHero)
            {
                UpdateHeroWithAttack(e, newHealth);
            }
            else
            {
                UpdateEnemyPlayerBox(e, newHealth);
            }
        }

        private static int CalculateHealth(UpdateEventArgs e)
        {
            int newHealth;
            if (e.Health <= 0)
            {
                newHealth = 0;
            }
            else
            {
                newHealth = e.Health * 10;
            }

            return newHealth;
        }

        private void ResetPictureBoxBackgrounds()
        {
            foreach(var heroPb in heroPbs)
            {
                heroPb.BackColor = Color.Transparent;
                heroPb.Update();
            }
            foreach(var enemyPb in enemyPbs)
            {
                enemyPb.BackColor = Color.Transparent;
                enemyPb.Update();
            }
        }

        private void UpdateEnemyPlayerBox(UpdateEventArgs e, int newHealth)
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
                enemyHealthBars[e.TurnTag - 1].Visible = false;
            }
        }

        private void UpdateHeroWithAttack(UpdateEventArgs e, int newHealth)
        {
            // if the target is still alive update bar else make them invisible
            if (newHealth > 0)
            {
                // heroPbs[e.TurnTag - 1].BackColor = Color.Yellow;
                heroHealthBars[e.TurnTag - 1].Value = newHealth; // HARD CODED HARD CODED
                heroPbs[e.TurnTag - 1].Update();
                heroHealthBars[e.TurnTag - 1].Update(); // HARD CODED HARD CODED

            }
            else
            {
                battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                heroPbs[e.TurnTag - 1].Visible = false;
                heroHealthBars[e.TurnTag - 1].Value = 0;
                heroHealthBars[e.TurnTag - 1].Visible = false;
            }
        }
        
        public void OnBeatEncounter_Handler(object sender, EventArgs e)
        {
            battleLog.AppendText("\r\nYou beat the encounter ! Great job !");
        }
        
        public void OnLostGame_Handler(object sender, EventArgs e)
        {
            battleLog.AppendText("\r\nGame lost :(");
            MessageBox.Show("You lost the game ! Good try !");
            // disable all buttons
            // RE ENABLED
            HideEnemyPictureBoxes();
            HideEnemyProgressBars();
            ToggleActionBtnEnable(false);
            ToggleEnemyPictureBoxEnable(false);
        }

        private void ToggleActionBtnEnable(bool newValue)
        {
            attackBtn.Enabled = newValue;
            defendBtn.Enabled = newValue;
            specialBtn.Enabled = newValue;
        }

        private void ToggleEnemyPictureBoxEnable(bool newValue)
        {
            foreach(var box in enemyPbs)
            {
                box.Enabled = newValue;
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
                    //RE ENABLE

                    foreach(var enemy in enemyPbs)
                    {
                        if(enemy.Visible)
                        {
                            enemy.BackColor = Color.Red;
                        }
                    }

                    break;
                case "Defend":
                    battleLog.AppendText("\r\nDefend has been chosen !");
                    queuedAction = "Defend";
                    // RE 
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

                    foreach (var enemy in enemyPbs)
                    {
                        enemy.BackColor = Color.Red;
                    }
                    battleLog.AppendText("\r\nSpecial has been chosen !");
                    break;

            }

            ToggleActionBtnEnable(false);

            // enable all enemy buttons so user can select enemy if defend was not selected
            if (queuedAction != "Defend")
            {
                foreach (var enemyBox in enemyPbs)
                {
                    if (enemyBox.Visible)
                    {
                        enemyBox.Enabled = true;
                    }
                }
            }
            // else re-enable attack so we can attack again
            else
            {
                ToggleActionBtnEnable(true);
            }
        }
        private void EnemyPbClicked_Handler(object sender, EventArgs e)
        {
            int targetTag = int.Parse((((PictureBox)sender).Tag).ToString());
            TurnReadyEventArgs args = new TurnReadyEventArgs();
            args.Attack = queuedAction;
            args.Enemy = targetTag;

            OnTurnReady(this, args);
            ToggleEnemyPictureBoxEnable(false);
            ToggleActionBtnEnable(true);
        }

        protected virtual void OnTurnReady(object sender, TurnReadyEventArgs e)
        {
            TurnReady.Invoke(this, e);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("high_score.txt");
            string highscore = reader.ReadLine();
            MessageBox.Show("Highscore : " + highscore);
        }
    }
}
