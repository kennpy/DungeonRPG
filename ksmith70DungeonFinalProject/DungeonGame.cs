using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ksmith70DungeonFinalProject
{
    public class DungeonGame
    {
        private GameScreen screen;
        private GameLogic logic = new GameLogic();

        public void Run()
        {
            // main game loop 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            screen = new GameScreen();
            // SubscribeEvents();
            // this is all we need 

            logic.Update += screen.OnUpdate_Handler;
            screen.TurnReady += logic.OnTurnReady_Handler;
            logic.NewEncounter += screen.OnNewEncounter_Handler;
            logic.CurrentAttacker += screen.OnPlayerChoice_Handler;
            logic.LostGame += screen.OnLostGame_Handler;
            logic.BeatEncounter += screen.OnBeatEncounter_Handler;

            logic.UpdateGUI();
            Application.Run(screen);



        }
    }
}