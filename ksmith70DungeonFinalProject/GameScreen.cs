using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    /// <summary>
    /// Author : Kenji Smith
    /// Game Screen which registers attack and enemy selections.
    /// Updates actor selections and health based on what logic passes.
    /// Highlights actors based on current turn order
    /// </summary>
    public partial class GameScreen : Form
    {
        private string queuedAction;
        public event EventHandler<TurnReadyEventArgs> TurnReady;

        List<PictureBox> heroPbs = new List<PictureBox>();
        List<PictureBox> enemyPbs = new List<PictureBox>();

        Bitmap[] heroSprites = new Bitmap[3];
        Bitmap[] enemySprites = new Bitmap[3];

        ProgressBar[] heroHealthBars = new ProgressBar[3];
        ProgressBar[] enemyHealthBars = new ProgressBar[3];
    
        /// <summary>
        /// Sets up the board components and handlers
        /// </summary>
        public GameScreen()
        {
            InitializeComponent();
            AssignPictureBoxes();
            SubscribeActionHandlers();
            InstantiateProgressBars();
        }

        /// <summary>
        /// Adds health bars to their respective heroes
        /// </summary>
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

        /// <summary>
        /// Subscribes actino buttons
        /// </summary>
        private void SubscribeActionHandlers()
        {
            attackBtn.Click += ActionButtonClick_Handler;
            defendBtn.Click += ActionButtonClick_Handler;
            specialBtn.Click += ActionButtonClick_Handler;
        }

        /// <summary>
        /// Adds hero / enemy boxes to their respective arrays
        /// </summary>
        private void AssignPictureBoxes()
        {
            // add hero and enemy picture boxes
            heroPbs.Add(heroPb1);
            heroPbs.Add(heroPb2);
            heroPbs.Add(heroPb3);

            enemyPbs.Add(enemyPb1);
            enemyPbs.Add(enemyPb2);
            enemyPbs.Add(enemyPb3);
        }

        /// <summary>
        /// Register than an ambush has taken place, updating battle log and picture boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNewEncounter_Handler(object sender, NewEncounterEventArgs e)
        {
            battleLog.AppendText("\r\nYou have been ambushed by some new enemies !");

            // hide by defualt so we can only enable the ones we need
            RemoveEnemyClickHandlers();
            HideEnemyPictureBoxes();
            HideEnemyProgressBars();

            // Enable ones that are valid
            for (int position = 0; position < e.HeroSprites.Count; position++)
            {
                SetInitialHeroState(e, position);
            }
        
            for (int position = 0; position < e.EnemySprites.Count; position++)
            {
                SetInitialEnemyState(e, position);
            }
        }

        /// <summary>
        /// Remove enemy picture box handlers from all enemy picture boxes
        /// </summary>
        private void RemoveEnemyClickHandlers()
        {
            foreach (var enemyBox in enemyPbs)
            {
                enemyBox.Click -= EnemyPbClicked_Handler;
            }
        }

        /// <summary>
        /// Make all enemy data visible, allow them to be clicked, and make the changes visible
        /// </summary>
        /// <param name="e"></param>
        /// <param name="position"></param>
        private void SetInitialEnemyState(NewEncounterEventArgs e, int position)
        {
            // Make enemies visible
            enemyPbs[position].Visible = true;
            enemyHealthBars[position].Visible = true;
            // Update their backgrounds / health status bars
            enemyPbs[position].BackgroundImage = e.EnemySprites[position];
            enemyHealthBars[position].Value = e.EnemyHealth[position] * 10;
            // Subscribe them and make sure they can be clicked and seen
            enemyPbs[position].Click += EnemyPbClicked_Handler;
            enemyPbs[position].Enabled = false;
            enemyPbs[position].Update();
            enemyHealthBars[position].Update();
        }

        /// <summary>
        /// Make all enemy progress bars invisible
        /// </summary>
        private void HideEnemyProgressBars()
        {
            foreach (var bar in enemyHealthBars)
            {
                bar.Visible = false;
            }
        }
        
        /// <summary>
        /// make ell enemy picture boxes invisible
        /// </summary>
        private void HideEnemyPictureBoxes()
        {
            foreach(var box in enemyPbs)
            {
                box.Visible = false;
            }
        }
       
        /// <summary>
        /// Make all attack buttons invisible
        /// </summary>
        private void HideAttackBtns()
        {
            attackBtn.Visible = false;
            defendBtn.Visible = false;
            specialBtn.Visible = false;
        }
        
        /// <summary>
        /// Sets background image and hero health before updating screen
        /// </summary>
        /// <param name="e"></param>
        /// <param name="position">Where the hero is located in the hero health list</param>
        private void SetInitialHeroState(NewEncounterEventArgs e, int position)
        {
            // if the hero is alive set their state and make them visible
            if (e.HeroHealth[position] > 0)
            {
                heroPbs[position].BackgroundImage = e.HeroSprites[position];
                heroHealthBars[position].Value = e.HeroHealth[position] * 10;
                heroPbs[position].Update();
                heroHealthBars[position].Update();
            }
        }

        /// <summary>
        /// Update hero / enemy picture box to display that they are attacking
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        public  void OnPlayerChoice_Handler(object sener, CurrentAttackerEventArgs e)
        {
            // enable action buttons and disable enemies
            ToggleActionBtnEnable(true);
            ResetPictureBoxBackgrounds();

            // update their backgrounds
            if (e.AttackerIsHero)
            {
                heroPbs[e.PlayerTag - 1].BackColor = Color.Yellow;
            }
            else
            {
                enemyPbs[e.PlayerTag - 1].BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// Register attack / defense and update screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnUpdate_Handler(object sender, UpdateEventArgs e)
        {

            // Update screen depending on actor / action
            if (e.TurnTag != -1)
            {
                if (!e.DefendWasChosen)
                {
                    ShowAttack(e);
                }
                else
                {
                    ShowDefense(e);
                }
            }
            else // set the initial background state
            {
                heroPbs[0].BackColor = Color.Yellow;
                heroPbs[0].Update();
            }
           
        }

        /// <summary>
        /// Register that a defense has occured in the battle log
        /// </summary>
        /// <param name="e"></param>
        private void ShowDefense(UpdateEventArgs e)
        {
            // if the hero has defended update the battle log 
            if (e.TargetIsHero)
            {
                battleLog.AppendText("\r\n" + e.AttackerName + " is defending !");
                battleLog.AppendText("\r\n" + e.TargetName + " defended !");
            }
        }

        /// <summary>
        /// Show that an attack has happened. Updates battle log and target health
        /// </summary>
        /// <param name="e"></param>
        private void ShowAttack(UpdateEventArgs e)
        {
            // register action on battle log
            battleLog.AppendText("\r\n" + e.AttackerName + " is attacking !");
            battleLog.AppendText("\r\n" + e.TargetName + " was attacked !");

            // update the target that was attacked
            int newHealth = CalculateHealth(e);
            if (e.TargetIsHero)
            {
                UpdateHeroWithAttack(e, newHealth);
            }
            else
            {
                UpdateEnemyStatus(e, newHealth);
            }
        }

        /// <summary>
        /// Calculate health to display based on current health
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Value of health to display</returns>
        private int CalculateHealth(UpdateEventArgs e)
        {
            // calculate the new health 
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

        /// <summary>
        /// Hides all picture boxes
        /// </summary>
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

        /// <summary>
        /// Modifies enemy health if alive, and death if health is below zero
        /// </summary>
        /// <param name="e"></param>
        /// <param name="newHealth">The new enemy health to show</param>
        private void UpdateEnemyStatus(UpdateEventArgs e, int newHealth)
        {
            // if they are alive update their health bar
            if (newHealth > 0)
            {
                enemyHealthBars[e.TurnTag - 1].Value = newHealth; 
                enemyHealthBars[e.TurnTag - 1].Update(); 
                enemyPbs[e.TurnTag - 1].Update();  
            }
            // else register their death and hide them
            else
            {
                battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                enemyPbs[e.TurnTag - 1].Visible = false;
                enemyHealthBars[e.TurnTag - 1].Value = 0;
                enemyHealthBars[e.TurnTag - 1].Visible = false;
            }
        }

        /// <summary>
        /// Update that hero has been attacked. Update health bar or registers if they were killed
        /// </summary>
        /// <param name="e"></param>
        /// <param name="newHealth">The health to update health bar with</param>
        private void UpdateHeroWithAttack(UpdateEventArgs e, int newHealth)
        {
            // if they are alive update their health bar
            if (newHealth > 0)
            {
                heroHealthBars[e.TurnTag - 1].Value = newHealth;
                heroPbs[e.TurnTag - 1].Update();
                heroHealthBars[e.TurnTag - 1].Update(); 

            }
            // else register their death and hide them
            else
            {
                battleLog.AppendText("\r\n" + e.TargetName + " was killed !");
                heroPbs[e.TurnTag - 1].Visible = false;
                heroHealthBars[e.TurnTag - 1].Value = 0;
                heroHealthBars[e.TurnTag - 1].Visible = false;
            }
        }
        
        /// <summary>
        /// Updates battle log to show that encounter was beat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnBeatEncounter_Handler(object sender, EventArgs e)
        {
            battleLog.AppendText("\r\nYou beat the encounter ! Great job !");
        }
        
        /// <summary>
        /// Registers that game was lost in battle log and hides all buttons / actors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLostGame_Handler(object sender, EventArgs e)
        {
            // update the battle log and show a message
            battleLog.AppendText("\r\nGame lost :(");
            MessageBox.Show("You lost the game ! Good try !");
            
            // disable all buttons / picture boxes
            HideEnemyPictureBoxes();
            HideEnemyProgressBars();
            HideAttackBtns();
            ToggleActionBtnEnable(false);
            ToggleEnemyPictureBoxEnable(false);
        }

        /// <summary>
        /// Toggles button enabled status based on supplied boolean
        /// </summary>
        /// <param name="newValue">The value to set action button enabled status to</param>
        private void ToggleActionBtnEnable(bool newValue)
        {
            attackBtn.Enabled = newValue;
            defendBtn.Enabled = newValue;
            specialBtn.Enabled = newValue;
        }

        /// <summary>
        /// Toggle picture box enabled status based on supplied boolean
        /// </summary>
        /// <param name="newValue">The value to set the enabled status to</param>
        private void ToggleEnemyPictureBoxEnable(bool newValue)
        {
            foreach(var box in enemyPbs)
            {
                box.Enabled = newValue;
            }
        }

        /// <summary>
        /// Handler for action buttons. Registers action in battle log and modifies enabled + visibility
        /// status of buttons and picture boxes based on what was selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionButtonClick_Handler(object sender, EventArgs e)
        {
            string action = ((Button)sender).Tag.ToString();
            RegisterAction(action);

            // hide / enable the correct buttons
            ToggleActionBtnEnable(false);
            
            // enable all enemy buttons so user can select enemy if defend was not selected
            if (queuedAction != "Defend")
            {
                EnableVisibleEnemies();
            }
            // else re-enable attack so we can attack again
            else
            {
                ToggleActionBtnEnable(true);
            }
        }

        /// <summary>
        /// Selects all enemies that are visible and enables them
        /// </summary>
        private void EnableVisibleEnemies()
        {
            foreach (var enemyBox in enemyPbs)
            {
                if (enemyBox.Visible)
                {
                    enemyBox.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Registers that an action has occurred 
        /// </summary>
        /// <param name="action">The action that has taken place</param>
        private void RegisterAction(string action)
        {
            switch (action)
            {
                case "Attack":
                    RegisterAttack();
                    break;
                case "Defend":
                    RegisterDefense();
                    break;
                case "Special":
                    RegisterSpecial();
                    break;
            }
        }

        /// <summary>
        /// Register a special attack has occurred
        /// </summary>
        private void RegisterSpecial()
        {
            queuedAction = "Special";
            foreach (var enemy in enemyPbs)
            {
                enemy.BackColor = Color.Red;
            }
            battleLog.AppendText("\r\nSpecial has been chosen !");
        }

        /// <summary>
        /// Register defense has occurred
        /// </summary>
        private void RegisterDefense()
        {
            battleLog.AppendText("\r\nDefend has been chosen !");
            queuedAction = "Defend";
            foreach (var enemy in enemyPbs)
            {
                enemy.BackColor = Color.Red;
            }
            TurnReadyEventArgs args = new TurnReadyEventArgs();
            args.Attack = queuedAction;
            args.Enemy = 1;
            OnTurnReady(this, args);
        }

        /// <summary>
        /// Register attack has occurred
        /// </summary>
        private void RegisterAttack()
        {
            battleLog.AppendText("\r\nAttack has been chosen !");
            queuedAction = "Attack";
            
            // show enemies that can be targeted
            foreach (var enemy in enemyPbs)
            {
                if (enemy.Visible)
                {
                    enemy.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// Extracts action / target and throws TurnReady event. 
        /// Hides / enables picture boxes and action buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnemyPbClicked_Handler(object sender, EventArgs e)
        {
            // Create and throw turn ready args
            int targetTag = int.Parse((((PictureBox)sender).Tag).ToString());
            TurnReadyEventArgs args = new TurnReadyEventArgs();
            args.Attack = queuedAction;
            args.Enemy = targetTag;
            OnTurnReady(this, args);

            // Hide enemy boxes and enable actions
            ToggleEnemyPictureBoxEnable(false);
            ToggleActionBtnEnable(true);
        }

        /// <summary>
        /// Invokes TurnReady event with the supplied arguments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnTurnReady(object sender, TurnReadyEventArgs e)
        {
            TurnReady.Invoke(this, e);

        }

        /// <summary>
        /// High score button click handler
        /// Displays current high score based on number of encounters won
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highScoreButton_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("high_score.txt");
            string highscore = reader.ReadLine();
            MessageBox.Show("Highscore : " + highscore);
        }
    }
}
