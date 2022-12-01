using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ksmith70DungeonFinalProject
{
    public class DungeonGame
    {
        private GameScreen screen;
        private GameLogic logic;

        public DungeonGame()
        {
            
            this.screen = new GameScreen();
            this.logic = new GameLogic();
        }

        public void Run()
        {

            // main game loop 
        }
    }
}