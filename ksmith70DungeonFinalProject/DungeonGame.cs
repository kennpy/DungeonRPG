using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            logic.Update += screen.OnUpdate_Handler;
            logic.UpdateGUI();
            // SubscribeEvents();
            Application.Run(screen);

        }
    }
}